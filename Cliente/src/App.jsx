import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import ProtectedRoute from './components/ProtectedRoute';
import ProductList from './components/ProductList';
import Cart from './components/Cart';
import { CartProvider } from './context/CartContext';
import './App.css';

function App() {
  return (
    <AuthProvider>
      <CartProvider>
        <Router>
          <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />

            <Route element={<ProtectedRoute />}>
              <Route path="/" element={
                <div className="dashboard">
                  <h1>Bienvenido a Firmeza Web</h1>
                  <p>Has iniciado sesión correctamente.</p>
                  <div className="main-content" style={{ display: 'flex', gap: '20px' }}>
                    <div className="catalog-section" style={{ flex: 2 }}>
                      <h2>Catálogo de Productos</h2>
                      <ProductList />
                    </div>
                    <div className="cart-section" style={{ flex: 1 }}>
                      <Cart />
                    </div>
                  </div>
                </div>
              } />
            </Route>
          </Routes>
        </Router>
      </CartProvider>
    </AuthProvider>
  );
}

export default App;
