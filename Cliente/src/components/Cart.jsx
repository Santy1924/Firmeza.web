import { useCart } from '../context/CartContext';

const Cart = () => {
    const { cartItems, removeFromCart, updateQuantity, subtotal, tax, total } = useCart();

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
                        <div className="item-controls">
                            <button onClick={() => updateQuantity(item.id, item.quantity - 1)}>-</button>
                            <span>{item.quantity}</span>
                            <button onClick={() => updateQuantity(item.id, item.quantity + 1)}>+</button>
                            <button className="remove-btn" onClick={() => removeFromCart(item.id)}>Eliminar</button>
                        </div>
                        <div className="item-total">
                            <p>${(item.precioUnitario * item.quantity).toFixed(2)}</p>
                        </div>
                    </div>
                ))}
            </div>
            <div className="cart-summary">
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
                <button className="checkout-btn">Proceder al Pago</button>
            </div>
        </div>
    );
};

export default Cart;
