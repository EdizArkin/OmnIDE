const { contextBridge } = require('electron');

const API_URL = 'http://localhost:5000';

contextBridge.exposeInMainWorld('api', {
    baseUrl: API_URL,
    async getProjects() {
        const response = await fetch(`${API_URL}/api/project`);
        return response.json();
    }
});
