import { useState } from 'react';
import { useCart } from '../context/CartContext';

const Cart = () => {
    const { cartItems, removeFromCart, updateQuantity, subtotal, tax, total, checkout } = useCart();
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState({ type: '', text: '' });

    const handleCheckout = async () => {
        setLoading(true);
        setMessage({ type: '', text: '' });

        try {
            const result = await checkout();
            setMessage({
                type: 'success',
                text: `¡Compra exitosa! Número de venta: ${result.id}`
            });

            // Clear message after 5 seconds
            setTimeout(() => setMessage({ type: '', text: '' }), 5000);
        } catch (error) {
            setMessage({
                type: 'error',
                text: error.message || 'Error al procesar el pago'
            });
        } finally {
            setLoading(false);
        }
    };

    if (cartItems.length === 0) {
        return (
            <div className="cart-container">
                <h2>Carrito de Compras</h2>
                <p>Tu carrito está vacío.</p>
            </div>
        );
    }

    return (
        <div className="cart-container">
            <h2>Carrito de Compras</h2>
            <div className="cart-items">
                {cartItems.map((item) => (
                    <div key={item.id} className="cart-item">
                        <div className="item-info">
                            <h4>{item.nombre}</h4>
                            <p>${item.precioUnitario}</p>
                        </div>
                        <div className="item-actions">
                            <div className="item-controls">
                                <button onClick={() => updateQuantity(item.id, item.quantity - 1)}>-</button>
                                <span>{item.quantity}</span>
                                <button onClick={() => updateQuantity(item.id, item.quantity + 1)}>+</button>
                            </div>
                            <div className="item-total">
                                <p>${(item.precioUnitario * item.quantity).toFixed(2)}</p>
                            </div>
                        </div>
                        <button className="remove-btn" onClick={() => removeFromCart(item.id)}>Eliminar</button>
                    </div>
                ))}
            </div>
            <div className="cart-summary">
                {message.text && (
                    <div className={`message ${message.type}`} style={{
                        padding: '10px',
                        marginBottom: '10px',
                        borderRadius: '4px',
                        backgroundColor: message.type === 'success' ? '#d4edda' : '#f8d7da',
                        color: message.type === 'success' ? '#155724' : '#721c24',
                        border: `1px solid ${message.type === 'success' ? '#c3e6cb' : '#f5c6cb'}`
                    }}>
                        {message.text}
                    </div>
                )}
                <div className="summary-row">
                    <span>Subtotal:</span>
                    <span>${subtotal.toFixed(2)}</span>
                </div>
                <div className="summary-row">
                    <span>Impuestos (19%):</span>
                    <span>${tax.toFixed(2)}</span>
                </div>
                <div className="summary-row total">
                    <span>Total:</span>
                    <span>${total.toFixed(2)}</span>
                </div>
                <button
                    className="checkout-btn"
                    onClick={handleCheckout}
                    disabled={loading || cartItems.length === 0}
                >
                    {loading ? 'Procesando...' : 'Proceder al Pago'}
                </button>
            </div>
        </div>
    );
};

export default Cart;
