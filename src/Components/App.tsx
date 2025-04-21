import React, { useEffect, useState } from 'react';

interface Project {
  id: number;
  name: string;
  description: string;
  createdDate: string;
  language: string;
  status: string;
}

// Add type definition for window.api
declare global {
  interface Window {
    api: {
      baseUrl: string;
      getProjects: () => Promise<Project[]>;
    }
  }
}

const App = () => {
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    window.api.getProjects()
      .then(data => {
        setProjects(data);
        setLoading(false);
      })
      .catch(error => {
        console.error('Error:', error);
        setError('Failed to connect to API. Please make sure the API is running.');
        setLoading(false);
      });
  }, []);

  if (loading) {
    return <div className="flex items-center justify-center h-screen">Loading...</div>;
  }

  if (error) {
    return (
      <div className="flex items-center justify-center h-screen">
        <div className="text-center text-red-600">
          <h2 className="text-xl font-bold mb-2">Error</h2>
          <p>{error}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="flex items-center justify-center h-screen">
      <div className="text-center">
        <h1 className="text-4xl font-bold text-blue-600">Welcome to OmnIDE</h1>
        <p className="mt-4 text-gray-600">Your next generation IDE</p>
        <div className="mt-8">
          {projects.map(project => (
            <div key={project.id} className="mt-4 p-4 border rounded">
              <h2 className="text-xl">{project.name}</h2>
              <p>{project.description}</p>
              <p className="text-sm text-gray-500">
                Language: {project.language} | Status: {project.status}
              </p>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default App;