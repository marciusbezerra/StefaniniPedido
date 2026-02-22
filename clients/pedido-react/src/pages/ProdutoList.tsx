import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Produto } from '../models/Produto';
import { ProdutoService } from '../services/produtoService';

export function ProdutoList() {
    const [produtos, setProdutos] = useState<Produto[]>([]);
    const [loading, setLoading] = useState(false);
    const [erro, setErro] = useState('');
    const navigate = useNavigate();

    const carregar = async () => {
        setLoading(true);
        try {
            setProdutos(await ProdutoService.getAll());
        } catch (error) {
            console.error('Erro ao carregar produtos:', error);
            setErro('Erro ao carregar produtos.');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => { carregar(); }, []);

    const deletar = async (id: number) => {
        if (!confirm('Confirma exclusão do produto?')) return;
        try {
            await ProdutoService.delete(id);
            setProdutos(produtos.filter(p => p.id !== id));
            // carregar();
        } catch (error) {
            console.error('Erro ao deletar produto:', error);
            setErro('Erro ao deletar produto.');
        }
    };

    return (
        <div className="container mt-4">
            <div className="d-flex justify-content-between align-items-center mb-3">
                <h2>Produtos</h2>
                <button className="btn btn-primary" onClick={() => navigate('/produtos/novo')}>+ Novo Produto</button>
            </div>
            {erro && <div className="alert alert-danger">{erro}</div>}
            {loading ? (
                <div className="text-center"><div className="spinner-border" /></div>
            ) : (
                <div className="table-responsive">
                    <table className="table table-striped">
                        <thead className="table-dark">
                            <tr><th>#</th><th>Nome</th><th>Valor</th><th>Ações</th></tr>
                        </thead>
                        <tbody>
                            {produtos.length === 0 ? (
                                <tr><td colSpan={4} className="text-center">Nenhum produto encontrado.</td></tr>
                            ) : produtos.map(p => (
                                <tr key={p.id}>
                                    <td>{p.id}</td>
                                    <td>{p.nomeProduto}</td>
                                    <td>{p.valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</td>
                                    <td>
                                        <button className="btn btn-sm btn-outline-primary me-1" onClick={() => navigate(`/produtos/editar/${p.id}`)}>Editar</button>
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
