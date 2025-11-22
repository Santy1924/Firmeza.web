import { useState, useEffect } from 'react';
import axios from '../api/axios';
import { useCart } from '../context/CartContext';

const ProductList = () => {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const { addToCart } = useCart();

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const response = await axios.get('/Producto');
                setProducts(response.data);
            } catch (err) {
                console.error('Error fetching products:', err);
                setError('No se pudieron cargar los productos.');
            } finally {
                setLoading(false);
            }
        };

        fetchProducts();
    }, []);

    if (loading) return <p>Cargando productos...</p>;
    if (error) return <p className="error">{error}</p>;

    return (
        <div className="product-list">
            {products.length === 0 ? (
                <p>No hay productos disponibles.</p>
            ) : (
                <div className="products-grid">
                    {products.map((product) => (
                        <div key={product.id} className="product-card">
                            <h3>{product.nombre}</h3>
                            <p>{product.descripcion}</p>
                            <p className="price">${product.precioUnitario}</p>
                            <button onClick={() => addToCart(product)}>Agregar al Carrito</button>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default ProductList;
