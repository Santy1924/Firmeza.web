import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import ProtectedRoute from './components/ProtectedRoute';
import ProductList from './components/ProductList';
import Cart from './components/Cart';
import { CartProvider } from './context/CartContext';
import './App.css';

function Dashboard() {
  const { logout, user } = useAuth();

  return (
    <div className="dashboard-container">
      <header className="dashboard-header">
        <div className="header-content">
          <h1>Firmeza</h1>
          <div className="user-info">
            <span>Hola, {user ? (user.name || user.email || 'Usuario') : 'Usuario'}</span>
            <button onClick={logout} className="btn-logout">
              Cerrar Sesión
            </button>
          </div>
        </div>
      </header>

      <main className="main-content">
        <div className="catalog-section">
          <h2>Catálogo de Productos</h2>
          <ProductList />
        </div>
        <div className="cart-section">
          <div className="sticky-cart">
            <Cart />
          </div>
        </div>
      </main>
    </div>
  );
}

function App() {
  return (
    <AuthProvider>
      <CartProvider>
        <Router>
          <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />

            <Route element={<ProtectedRoute />}>
              <Route path="/" element={<Dashboard />} />
            </Route>
          </Routes>
        </Router>
      </CartProvider>
    </AuthProvider>
  );
}

export default App;
