import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Pedido } from '../models/Pedido';
import { PedidoService } from '../services/pedidoService';

export function PedidoDetalhes() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [pedido, setPedido] = useState<Pedido | null>(null);
    const [loading, setLoading] = useState(false);
    const [erro, setErro] = useState('');

    useEffect(() => {
        if (id) {
            setLoading(true);
            PedidoService.getById(Number(id)).then(p => {
                setPedido(p);
                setLoading(false);
            }).catch(error => {
                console.error('Erro ao carregar pedido:', error);
                setErro('Erro ao carregar pedido.');
                setLoading(false);
            });
        }
    }, [id]);

    return (
        <div className="container mt-4" style={{ maxWidth: 700 }}>
            <h2 className='mb-4'>
                Detalhes do Pedido #{id}
            </h2>
            {erro && <div className="alert alert-danger">{erro}</div>}
            {loading ? (
                <div className="text-center"><div className="spinner-border" /></div>
            ) : pedido ? (
                <div>
                    <p><strong>Cliente:</strong> {pedido.nomeCliente}</p>
                    <p><strong>Email:</strong> {pedido.emailCliente}</p>
                    <p><strong>Valor Total:</strong> {pedido.valorTotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</p>
                    <p><strong>Pago:</strong> <span className={`badge ${pedido.pago ? 'bg-success' : 'bg-warning'}`}>{pedido.pago ? 'Sim' : 'Não'}</span></p>
                    <h5>Itens do Pedido</h5>
                    <ul className="list-group">
                        {pedido.itensPedido.map(item => (
                            <li key={item.id} className="list-group-item d-flex justify-content-between align-items-center">
                                <div>
                                    {item.nomeProduto} (x{item.quantidade})
                                </div>
                                <span>{(item.valorUnitario * item.quantidade).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</span>
                            </li>
                        ))}
                        <li className="list-group-item d-flex justify-content-between align-items-center">
                            <strong>Total</strong>
                            <strong>{pedido.valorTotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</strong>
                        </li>
                    </ul>
                    <div className="mt-3">
                        <button className="btn btn-secondary" onClick={() => navigate('/pedidos')}>Voltar</button>
                    </div>
                </div>
            ) : (
                !erro && (
                    <div className="alert alert-info">Pedido não encontrado.</div>
                )
            )}
        </div>
    );
}
