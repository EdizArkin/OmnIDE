const fs = require('fs');
const path = require('path');
const https = require('https');
const { execSync } = require('child_process');

const PYTHON_VERSION = '3.9.13';
const PYTHON_EMBED_URL = `https://www.python.org/ftp/python/${PYTHON_VERSION}/python-${PYTHON_VERSION}-embed-amd64.zip`;
const PYTHON_DIR = path.join(__dirname, '..', 'OmnIDEApi', 'Python', 'python-embed');

async function downloadPython() {
    try {
        if (fs.existsSync(PYTHON_DIR)) {
            fs.rmSync(PYTHON_DIR, { recursive: true, force: true });
        }
        fs.mkdirSync(PYTHON_DIR, { recursive: true });

        const zipPath = path.join(PYTHON_DIR, 'python-embed.zip');

        return new Promise((resolve, reject) => {
            const file = fs.createWriteStream(zipPath);
            https.get(PYTHON_EMBED_URL, (response) => {
                response.pipe(file);
                file.on('close', async () => {
                    try {
                        await new Promise(r => setTimeout(r, 1000)); // wait for file close
                        const command = `powershell -Command "Expand-Archive -LiteralPath '${zipPath}' -DestinationPath '${PYTHON_DIR}' -Force"`;
                        execSync(command);
                        fs.unlinkSync(zipPath);
                        resolve();
                    } catch (error) {
                        console.error('Error during extraction:', error);
                        reject(error);
                    }
                });
            }).on('error', (error) => {
                console.error('Error downloading Python:', error);
                reject(error);
            });
        });
    } catch (error) {
        console.error('Error in downloadPython:', error);
        throw error;
    }
}

downloadPython().catch(console.error);
