import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { Navbar } from './components/Navbar';
import { PedidoList } from './pages/PedidoList';
import { PedidoForm } from './pages/PedidoForm';
import { ProdutoList } from './pages/ProdutoList';
import { ProdutoForm } from './pages/ProdutoForm';

function App() {
    return (
        <BrowserRouter>
            <Navbar />
            <main>
                <Routes>
                    <Route path="/" element={<Navigate to="/pedidos" replace />} />
                    <Route path="/pedidos" element={<PedidoList />} />
                    <Route path="/pedidos/novo" element={<PedidoForm />} />
                    <Route path="/pedidos/editar/:id" element={<PedidoForm />} />
                    <Route path="/produtos" element={<ProdutoList />} />
                    <Route path="/produtos/novo" element={<ProdutoForm />} />
                    <Route path="/produtos/editar/:id" element={<ProdutoForm />} />
                </Routes>
            </main>
        </BrowserRouter>
    );
}

export default App;
