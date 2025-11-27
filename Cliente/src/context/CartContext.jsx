import { createContext, useState, useContext, useEffect } from 'react';
import axios from '../api/axios';

const CartContext = createContext();

export const useCart = () => {
    return useContext(CartContext);
};

export const CartProvider = ({ children }) => {
    const [cartItems, setCartItems] = useState([]);
    const [subtotal, setSubtotal] = useState(0);
    const [tax, setTax] = useState(0);
    const [total, setTotal] = useState(0);

    const TAX_RATE = 0.19; // 19% tax

    useEffect(() => {
        calculateTotals();
    }, [cartItems]);

    const calculateTotals = () => {
        const newSubtotal = cartItems.reduce((sum, item) => sum + (item.precioUnitario * item.quantity), 0);
        const newTax = newSubtotal * TAX_RATE;
        const newTotal = newSubtotal + newTax;

        setSubtotal(newSubtotal);
        setTax(newTax);
        setTotal(newTotal);
    };

    const addToCart = (product) => {
        setCartItems((prevItems) => {
            const existingItem = prevItems.find((item) => item.id === product.id);
            if (existingItem) {
                return prevItems.map((item) =>
                    item.id === product.id
                        ? { ...item, quantity: item.quantity + 1 }
                        : item
                );
            } else {
                return [...prevItems, { ...product, quantity: 1 }];
            }
        });
    };

    const removeFromCart = (productId) => {
        setCartItems((prevItems) => prevItems.filter((item) => item.id !== productId));
    };

    const updateQuantity = (productId, quantity) => {
        if (quantity < 1) {
            removeFromCart(productId);
            return;
        }
        setCartItems((prevItems) =>
            prevItems.map((item) =>
                item.id === productId ? { ...item, quantity: parseInt(quantity) } : item
            )
        );
    };

    const clearCart = () => {
        setCartItems([]);
    };

    const checkout = async (metodoPago = 'Efectivo', tipoVenta = 'Contado') => {
        if (cartItems.length === 0) {
            throw new Error('El carrito está vacío');
        }

        const checkoutData = {
            metodoPago,
            tipoVenta,
            items: cartItems.map(item => ({
                productoId: item.id,
                cantidad: item.quantity
            }))
        };

        try {
            const response = await axios.post('/Venta/checkout', checkoutData);
            clearCart();
            return response.data;
        } catch (error) {
            console.error('Checkout failed:', error);
            const errorMessage = error.response?.data?.mensaje || error.response?.data || error.message || 'Error al procesar el pago';
            throw new Error(errorMessage);
        }
    };

    const value = {
        cartItems,
        subtotal,
        tax,
        total,
        addToCart,
        removeFromCart,
        updateQuantity,
        clearCart,
        checkout
    };

    return (
        <CartContext.Provider value={value}>
            {children}
        </CartContext.Provider>
    );
};
