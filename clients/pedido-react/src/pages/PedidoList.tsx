import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Pedido } from '../models/Pedido';
import { PedidoService } from '../services/pedidoService';

export function PedidoList() {
    const [pedidos, setPedidos] = useState<Pedido[]>([]);
    const [loading, setLoading] = useState(false);
    const [erro, setErro] = useState('');
    const navigate = useNavigate();

    const carregar = async () => {
        setLoading(true);
        try {
            setPedidos(await PedidoService.getAll());
        } catch (error) {
            console.error('Erro ao carregar pedidos:', error);
            setErro('Erro ao carregar pedidos.');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => { carregar(); }, []);

    const deletar = async (id: number) => {
        if (!confirm('Confirma exclusão do pedido?')) return;
        try {
            await PedidoService.delete(id);
            setPedidos(pedidos.filter(p => p.id !== id));
            // carregar();
        } catch (error) {
            console.error('Erro ao deletar pedido:', error);
            setErro('Erro ao deletar pedido.');
        }
    };

    return (
        <div className="container mt-4">
            <div className="d-flex justify-content-between align-items-center mb-3">
                <h2>Pedidos</h2>
                <button className="btn btn-primary" onClick={() => navigate('/pedidos/novo')}>+ Novo Pedido</button>
            </div>
            {erro && <div className="alert alert-danger">{erro}</div>}
            {loading ? (
                <div className="text-center"><div className="spinner-border" /></div>
            ) : (
                <div className="table-responsive">
                    <table className="table table-striped">
                        <thead className="table-dark">
                            <tr><th>#</th><th>Cliente</th><th>Email</th><th>Valor Total</th><th>Pago</th><th>Ações</th></tr>
                        </thead>
                        <tbody>
                            {pedidos.length === 0 ? (
                                <tr><td colSpan={6} className="text-center">Nenhum pedido encontrado.</td></tr>
                            ) : pedidos.map(p => (
                                <tr key={p.id}>
                                    <td>{p.id}</td>
                                    <td>{p.nomeCliente}</td>
                                    <td>{p.emailCliente}</td>
                                    <td>{p.valorTotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</td>
                                    <td><span className={`badge ${p.pago ? 'bg-success' : 'bg-warning'}`}>{p.pago ? 'Sim' : 'Não'}</span></td>
                                    <td>
                                        <button className="btn btn-sm btn-outline-primary me-1" onClick={() => navigate(`/pedidos/editar/${p.id}`)}>Editar</button>
                                        <button className="btn btn-sm btn-outline-danger" onClick={() => deletar(p.id)}>Excluir</button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </div>
    );
}
