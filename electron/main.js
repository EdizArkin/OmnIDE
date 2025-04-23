const { app, BrowserWindow, dialog } = require('electron');
const { spawn, exec } = require('child_process');
const path = require('path');
const fs = require('fs');
const isDev = process.env.NODE_ENV === 'development';

let apiProcess = null;
let viteProcess = null;

// Function to kill process on a specific port
function killProcessOnPort(port) {
  return new Promise((resolve) => {
    const command = process.platform === 'win32'
      ? `netstat -ano | findstr :${port}`
      : `lsof -i :${port} -t`;

    exec(command, (err, stdout) => {
      if (stdout) {
        const pid = process.platform === 'win32'
          ? stdout.split('\n')[0].split(/\s+/)[5]
          : stdout.trim();

        if (pid) {
          try {
            process.kill(pid, 'SIGTERM');
            console.log(`Process on port ${port} killed`);
          } catch (e) {
            console.error(`Failed to kill process on port ${port}:`, e);
          }
        }
      }
      resolve();
    });
  });
}

// Update cleanup function to return a promise
function cleanup() {
  return new Promise(async (resolve) => {
    try {
      // Kill API process first
      if (apiProcess) {
        try {
          apiProcess.kill('SIGTERM');
          console.log('API process kill signal sent');
        } catch (error) {
          console.error('Error killing API process:', error);
        }
      }

      // Wait for API to cleanup
      await new Promise(resolve => setTimeout(resolve, 500));

      // Kill processes on specific ports
      await killProcessOnPort(5000); // .NET API
      if (isDev) {
        await killProcessOnPort(5173); // Vite
      }

      // Final cleanup after everything else
      setTimeout(() => {
        try {
          // Force kill API if still running
          if (apiProcess && !apiProcess.killed) {
            process.kill(apiProcess.pid, 'SIGKILL');
          }
        } catch (error) {
          console.error('Error in final cleanup:', error);
        } finally {
          resolve();
        }
      }, 500);
    } catch (error) {
      console.error('Cleanup error:', error);
      resolve(); // Always resolve to ensure app can quit
    }
  });
}

function startApi() {
  const apiPath = isDev 
    ? path.join(__dirname, '../release/api/OmnIDEApi.exe')
    : path.join(process.resourcesPath, 'api/OmnIDEApi.exe');  // Changed this line

  console.log('Process resourcesPath:', process.resourcesPath); // Add debug logging
  console.log('API Path:', apiPath);
/*
  if (!fs.existsSync(apiPath)) {
    console.error('API not found at:', apiPath);
    dialog.showErrorBox('API Error', `API executable not found at: ${apiPath}\nPlease build the API first.`);
    app.quit();
    return null;
  }
*/
  apiProcess = spawn(apiPath);
/*
  apiProcess.stdout.on('data', (data) => {
    console.log(`API: ${data}`);
  });

  apiProcess.stderr.on('data', (data) => {
    console.error(`API Error: ${data}`);
    dialog.showErrorBox('API Error', `API Error: ${data}`);
  });

  apiProcess.on('error', (err) => {
    console.error('Failed to start API:', err);
    dialog.showErrorBox('API Error', 'Failed to start API. Please check if .NET Runtime is installed.');
    app.quit();
  });
*/

  return apiProcess;
}

function createWindow() {
  const mainWindow = new BrowserWindow({
    width: 1200,
    height: 800,
    webPreferences: {
      nodeIntegration: false,
      contextIsolation: true,
      sandbox: true,
      webSecurity: true,
      preload: path.join(__dirname, 'preload.js')
    }
  });

  // In development load from Vite, in production load the built file
  if (isDev) {
    mainWindow.loadURL('http://localhost:5173');
  } else {
    mainWindow.loadFile(path.join(__dirname, '../dist/index.html'));
  }

  // Set CSP headers
  mainWindow.webContents.session.webRequest.onHeadersReceived((details, callback) => {
    callback({
      responseHeaders: {
        ...details.responseHeaders,
        'Content-Security-Policy': [
          "default-src 'self'; " +
          "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
          "style-src 'self' 'unsafe-inline'; " +
          "connect-src 'self' http://localhost:5000 http://localhost:5173"
        ]
      }
    });
  });

  if (isDev) {
    mainWindow.webContents.openDevTools();
  }
}

// Update app.whenReady() to check API status
app.whenReady().then(() => {
  const api = startApi();
  if (api) {
    createWindow();
  }

  app.on('activate', () => {
    if (BrowserWindow.getAllWindows().length === 0) {
      createWindow();
    }
  });
});

// Update quit handlers
app.on('before-quit', async (e) => {
  e.preventDefault();
  try {
    await cleanup();
  } catch (error) {
    console.error('Error during cleanup:', error);
  } finally {
    app.exit(0);
  }
});

app.on('window-all-closed', async () => {
  try {
    await cleanup();
  } catch (error) {
    console.error('Error during cleanup:', error);
  } finally {
    if (process.platform !== 'darwin') {
      app.quit();
    }
  }
});

// Add cleanup on process exit
process.on('SIGINT', async () => {
  await cleanup();
  process.exit(0);
});

process.on('SIGTERM', async () => {
  await cleanup();
  process.exit(0);
});