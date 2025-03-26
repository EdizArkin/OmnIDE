import React, { useState, useEffect } from 'react';
import axios from 'axios';
import '../App.css';

const ConfigScreen = () => {
  const [config, setConfig] = useState({ setting1: '', setting2: 0 });
  const [loading, setLoading] = useState(true);
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');

  // Component yüklendiğinde mevcut yapılandırmayı getir
  useEffect(() => {
    axios.get('/api/configuration')
      .then(response => {
        setConfig(response.data);
        setLoading(false);
      })
      .catch(error => {
        console.error("Konfigürasyon getirilemedi: ", error);
        setLoading(false);
        setError('Konfigürasyon getirilemedi.');
      });
  }, []);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setConfig(prev => ({
      ...prev,
      [name]: name === 'setting2' ? (isNaN(value) ? 0 : parseInt(value, 10)) : value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    axios.post('/api/configuration', config)
      .then(response => {
        setConfig(response.data);
        setMessage('Yapılandırma başarıyla güncellendi.');
        setError('');
      })
      .catch(error => {
        console.error("Güncelleme hatası: ", error);
        setMessage('');
        setError('Yapılandırma güncellenirken hata oluştu.');
      });
  };

  if (loading) return <div>Yükleniyor...</div>;

  return (
    <div>
      <h2>Yapılandırma Ayarları</h2>
      {message && <p style={{ color: 'green' }}>{message}</p>}
      {error && <p style={{ color: 'red' }}>{error}</p>}
      <form onSubmit={handleSubmit}>
        <div>
          <label>Setting 1:</label>
          <input
            type="text"
            name="setting1"
            value={config.setting1}
            onChange={handleInputChange}
          />
        </div>
        <div>
          <label>Setting 2:</label>
          <input
            type="number"
            name="setting2"
            value={config.setting2}
            onChange={handleInputChange}
          />
        </div>
        <button type="submit">Kaydet</button>
      </form>
    </div>
  );
};

export default ConfigScreen;
