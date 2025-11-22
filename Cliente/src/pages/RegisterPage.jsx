import { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { useNavigate, Link } from 'react-router-dom';

const RegisterPage = () => {
    const [formData, setFormData] = useState({
        nombreCompleto: '',
        documento: '',
        correo: '',
        telefono: '',
        direccion: '',
        password: '',
        activo: true
    });
    const [error, setError] = useState('');
    const { register } = useAuth();
    const navigate = useNavigate();

    const handleChange = (e) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        try {
            await register(formData);
            alert('Registro exitoso. Por favor inicia sesión.');
            navigate('/login');
        } catch (err) {
            console.error(err);
            setError('Error al registrarse. Verifique los datos.');
        }
    };

    return (
        <div className="auth-container">
            <h2>Registro de Cliente</h2>
            {error && <p className="error">{error}</p>}
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label>Nombre Completo:</label>
                    <input name="nombreCompleto" onChange={handleChange} required />
                </div>
                <div className="form-group">
                    <label>Documento:</label>
                    <input name="documento" onChange={handleChange} required />
                </div>
                <div className="form-group">
                    <label>Correo:</label>
                    <input name="correo" type="email" onChange={handleChange} required />
                </div>
                <div className="form-group">
                    <label>Teléfono:</label>
                    <input name="telefono" onChange={handleChange} required />
                </div>
                <div className="form-group">
                    <label>Dirección:</label>
                    <input name="direccion" onChange={handleChange} required />
                </div>
                <div className="form-group">
                    <label>Contraseña:</label>
                    <input name="password" type="password" onChange={handleChange} required />
                </div>
                <button type="submit">Registrarse</button>
            </form>
            <p>
                ¿Ya tienes cuenta? <Link to="/login">Inicia sesión aquí</Link>
            </p>
        </div>
    );
};

export default RegisterPage;
