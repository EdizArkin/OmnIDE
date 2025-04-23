const fs = require('fs');
const path = require('path');
const https = require('https');
const { execSync } = require('child_process');

console.log('Script started.'); // Debug log

const PYTHON_VERSION = '3.9.13';
const PYTHON_EMBED_URL = `https://www.python.org/ftp/python/${PYTHON_VERSION}/python-${PYTHON_VERSION}-embed-amd64.zip`;
const PYTHON_DIR = path.join(__dirname, '..', 'OmnIDEApi', 'Python', 'python-embed');
const PYTHON_LIB_DIR = path.join(PYTHON_DIR, 'Lib', 'site-packages');
const REQUIREMENTS_FILE = path.join(__dirname, '..', 'OmnIDEApi', 'Python', 'requirements.txt');

async function downloadPython() {
    console.log('Starting downloadPython function.'); // Debug log
    try {
        if (fs.existsSync(PYTHON_DIR)) {
            console.log('Python directory exists. Removing it.'); // Debug log
            fs.rmSync(PYTHON_DIR, { recursive: true, force: true });
        }
        console.log('Creating Python directory.'); // Debug log
        fs.mkdirSync(PYTHON_DIR, { recursive: true });

        const zipPath = path.join(PYTHON_DIR, 'python-embed.zip');
        console.log(`Downloading Python from ${PYTHON_EMBED_URL} to ${zipPath}`); // Debug log

        return new Promise((resolve, reject) => {
            const file = fs.createWriteStream(zipPath);
            https.get(PYTHON_EMBED_URL, (response) => {
                response.pipe(file);
                file.on('close', async () => {
                    console.log('Download complete. Extracting archive.'); // Debug log
                    try {
                        await new Promise(r => setTimeout(r, 1000)); // wait for file close
                        const command = `powershell -Command "Expand-Archive -LiteralPath '${zipPath}' -DestinationPath '${PYTHON_DIR}' -Force"`;
                        execSync(command);
                        fs.unlinkSync(zipPath);

                        console.log('Extraction complete. Installing pip.'); // Debug log
                        await installPip();
                        console.log('Pip installation complete. Creating Python Lib directory.'); // Debug log
                        await createPythonLibDir();
                        console.log('Python Lib directory created. Installing requirements.'); // Debug log
                        await installRequirements();
                        console.log('Requirements installed successfully.'); // Debug log

                        resolve();
                    } catch (error) {
                        console.error('Error during extraction or installation:', error);
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

// Create Lib/site-packages directory
function createPythonLibDir() {
    console.log('Starting createPythonLibDir function.'); // Debug log
    return new Promise((resolve, reject) => {
        try {
            if (!fs.existsSync(PYTHON_LIB_DIR)) {
                fs.mkdirSync(PYTHON_LIB_DIR, { recursive: true });
                console.log('Created Lib/site-packages directory.');
            } else {
                console.log('Lib/site-packages directory already exists.');
            }
            resolve();
        } catch (error) {
            console.error('Error creating Lib/site-packages directory:', error);
            reject(error);
        }
    });
}

// Install requirements from requirements.txt
function installRequirements() {
    console.log('Starting installRequirements function.'); // Debug log
    return new Promise((resolve, reject) => {
        const pipExe = `"${path.join(PYTHON_DIR, 'Scripts', 'pip.exe')}"`; // Use full path to pip
        const requirementsFile = `"${REQUIREMENTS_FILE}"`; // Ensure quotes
        const targetDir = `"${PYTHON_LIB_DIR}"`; // Ensure quotes

        if (fs.existsSync(REQUIREMENTS_FILE)) {
            const command = `${pipExe} install -r ${requirementsFile} --target=${targetDir}`;
            try {
                console.log('Installing packages from requirements.txt...');
                execSync(command, { stdio: 'inherit' });
                console.log('Verifying installed packages...');
                const verifyCommand = `${pipExe} list --format=columns --path=${targetDir}`;
                execSync(verifyCommand, { stdio: 'inherit' }); // Verify installed packages
                resolve();
            } catch (error) {
                console.error('Error installing requirements:', error);
                reject(error);
            }
        } else {
            console.log('requirements.txt not found.');
            resolve();
        }
    });
}

// Install pip using get-pip.py
function installPip() {
    console.log('Starting installPip function.'); // Debug log
    return new Promise((resolve, reject) => {
        const pythonExe = `"${path.join(PYTHON_DIR, 'python.exe')}"`; // Ensure quotes
        const getPipScriptUrl = 'https://bootstrap.pypa.io/get-pip.py';
        const getPipScriptPath = `"${path.join(PYTHON_DIR, 'get-pip.py')}"`; // Ensure quotes

        https.get(getPipScriptUrl, (response) => {
            const file = fs.createWriteStream(getPipScriptPath.replace(/"/g, '')); // Remove quotes for fs
            response.pipe(file);
            file.on('close', () => {
                try {
                    const command = `${pythonExe} ${getPipScriptPath}`;
                    execSync(command, { stdio: 'inherit' });
                    fs.unlinkSync(getPipScriptPath.replace(/"/g, '')); // Remove quotes for fs
                    console.log('pip installed successfully.');
                    resolve();
                } catch (error) {
                    console.error('Error installing pip:', error);
                    reject(error);
                }
            });
        }).on('error', (error) => {
            console.error('Error downloading get-pip.py:', error);
            reject(error);
        });
    });
}

// Run it all
console.log('Starting the script execution.'); // Debug log
downloadPython().catch(console.error);
console.log('Script execution finished.'); // Debug log
