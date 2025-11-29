import { useState, useEffect } from 'react';
import axios from '../api/axios';
import { useNavigate } from 'react-router-dom';

const MyPurchases = () => {
    const [purchases, setPurchases] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        const fetchPurchases = async () => {
            try {
                const response = await axios.get('/Venta/mis-compras');
                setPurchases(response.data);
            } catch (err) {
                console.error('Error fetching purchases:', err);
                setError('No se pudieron cargar tus compras.');
            } finally {
                setLoading(false);
            }
        };

        fetchPurchases();
    }, []);

    return (
        <div className="dashboard-container">
            <header className="dashboard-header">
                <div className="header-content">
                    <h1>Mis Compras</h1>
                    <button onClick={() => navigate('/')} className="btn-logout" style={{ border: 'none', background: 'transparent', color: 'var(--primary)', cursor: 'pointer' }}>
                        ← Volver al Catálogo
                    </button>
                </div>
            </header>

            <main className="main-content" style={{ display: 'block' }}>
                <div className="cart-container" style={{ maxWidth: '800px', margin: '0 auto' }}>
                    <h2>Historial de Compras</h2>

                    {loading && <p>Cargando...</p>}
                    {error && <p className="error-message">{error}</p>}

                    {!loading && !error && purchases.length === 0 && (
                        <p>No has realizado ninguna compra aún.</p>
                    )}

                    {!loading && purchases.length > 0 && (
                        <div className="purchases-list">
                            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                                <thead>
                                    <tr style={{ borderBottom: '2px solid #eee', textAlign: 'left' }}>
                                        <th style={{ padding: '10px' }}>ID Venta</th>
                                        <th style={{ padding: '10px' }}>Fecha</th>
                                        <th style={{ padding: '10px' }}>Método Pago</th>
                                        <th style={{ padding: '10px' }}>Tipo Venta</th>
                                        <th style={{ padding: '10px' }}>Total</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {purchases.map((purchase) => (
                                        <tr key={purchase.id} style={{ borderBottom: '1px solid #f3f4f6' }}>
                                            <td style={{ padding: '15px 10px' }}>#{purchase.id}</td>
                                            <td style={{ padding: '15px 10px' }}>{new Date(purchase.fecha).toLocaleDateString()}</td>
                                            <td style={{ padding: '15px 10px' }}>{purchase.metodoPago}</td>
                                            <td style={{ padding: '15px 10px' }}>{purchase.tipoVenta}</td>
                                            <td style={{ padding: '15px 10px', fontWeight: 'bold' }}>${purchase.total}</td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    )}
                </div>
            </main>
        </div>
    );
};

export default MyPurchases;
