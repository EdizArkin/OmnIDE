import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { spawn } from 'child_process';

export default defineConfig({
  plugins: [
    react(),
    {
      name: 'electron-start',
      configureServer(server) {
        let electronProcess: ReturnType<typeof spawn> | null = null;

        server.httpServer?.on('listening', () => {
          if (electronProcess) {
            electronProcess.kill();
          }

          electronProcess = spawn('npx', ['electron', '.'], {
            stdio: 'inherit',
            shell: true,
          });

          electronProcess.on('close', () => {
            electronProcess = null;
          });
        });

        process.on('exit', () => {
          if (electronProcess) {
            electronProcess.kill();
          }
        });
      },
    },
  ],
  build: {
    outDir: 'dist',
  },
});
