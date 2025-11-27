import { useState, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import { useNavigate, Link } from 'react-router-dom';

const LoginPage = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const { login } = useAuth();
    const navigate = useNavigate();

    // Check for stored error on mount
    useEffect(() => {
        const storedError = sessionStorage.getItem('loginError');
        if (storedError) {
            setError(storedError);
            sessionStorage.removeItem('loginError');
        }
    }, []);

    const handleSubmit = async (e) => {
        e.preventDefault();
        console.log("Form submitted");
        setError('');
        setLoading(true);
        try {
            console.log("Attempting login...");
            await login(email, password);
            console.log("Login successful, navigating...");
            navigate('/');
        } catch (err) {
            console.log("ERROR CATCH LOGINPAGE", err);
            console.log("Error message:", err.message);
            const errorMsg = err.message || 'Credenciales inválidas o error en el servidor';
            console.log("Setting error state to:", errorMsg);
            // Store in sessionStorage in case of reload
            sessionStorage.setItem('loginError', errorMsg);
            setError(errorMsg);
        } finally {
            setLoading(false);
            console.log("Loading set to false");
        }
    };

    return (
        <div className="login-container">
            <div className="login-card">
                <h2>Iniciar Sesión</h2>
                {error && <div className="error-message">{error}</div>}
                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label>Email</label>
                        <input
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                            placeholder="ejemplo@correo.com"
                        />
                    </div>
                    <div className="form-group">
                        <label>Contraseña</label>
                        <input
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                            placeholder="********"
                        />
                    </div>
                    <button type="submit" className="login-btn" disabled={loading}>
                        {loading ? 'Ingresando...' : 'Ingresar'}
                    </button>
                </form>
                <div className="register-link">
                    ¿No tienes cuenta? <Link to="/register">Regístrate aquí</Link>
                </div>
            </div>
        </div>
    );
};

export default LoginPage;
