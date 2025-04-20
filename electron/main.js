const { app, BrowserWindow, dialog } = require('electron');
const { spawn } = require('child_process');
const path = require('path');
const fs = require('fs');
const isDev = process.env.NODE_ENV === 'development';

let apiProcess = null;

function startApi() {
  const apiPath = isDev 
    ? path.join(__dirname, '../OmnIDEApi/bin/Debug/net8.0/OmnIDEApi.exe')
    : path.join(process.resourcesPath, 'api/OmnIDEApi.exe');  // Changed this line

  console.log('Process resourcesPath:', process.resourcesPath); // Add debug logging
  console.log('API Path:', apiPath);

  if (!fs.existsSync(apiPath)) {
    console.error('API not found at:', apiPath);
    dialog.showErrorBox('API Error', `API executable not found at: ${apiPath}\nPlease build the API first.`);
    app.quit();
    return null;
  }

  apiProcess = spawn(apiPath);

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

  apiProcess.on('exit', (code) => {
    console.log(`API process exited with code ${code}`);
    if (code !== 0) {
      dialog.showErrorBox('API Error', `API process exited with code ${code}`);
      app.quit();
    }
  });

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

// Make sure to kill API process when app quits
app.on('before-quit', () => {
  if (apiProcess) {
    apiProcess.kill();
  }
});

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});
