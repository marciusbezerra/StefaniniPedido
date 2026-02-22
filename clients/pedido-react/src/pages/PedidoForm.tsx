import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useForm, useFieldArray } from 'react-hook-form';
import { CreatePedido, UpdatePedido } from '../models/Pedido';
import { Produto } from '../models/Produto';
import { PedidoService } from '../services/pedidoService';
import { ProdutoService } from '../services/produtoService';

export function PedidoForm() {
    const { id } = useParams<{ id: string }>();
    const isEdit = Boolean(id);
    const navigate = useNavigate();
    const [produtos, setProdutos] = useState<Produto[]>([]);
    const [loading, setLoading] = useState(false);
    const [erro, setErro] = useState('');

    const { register, handleSubmit, control, reset, formState: { errors } } = useForm<CreatePedido & UpdatePedido>({
        defaultValues: { nomeCliente: '', emailCliente: '', pago: false, itensPedido: [{ idProduto: 0, quantidade: 1 }] }
    });

    const { fields, append, remove } = useFieldArray({ control, name: 'itensPedido' as never });

    useEffect(() => {
        ProdutoService.getAll().then(setProdutos);
        if (isEdit && id) {
            PedidoService.getById(Number(id)).then(p => reset({ nomeCliente: p.nomeCliente, emailCliente: p.emailCliente, pago: p.pago }));
        }
    }, [id]);

    const onSubmit = async (data: CreatePedido & UpdatePedido) => {
        setLoading(true);
        try {
            if (isEdit) {
                await PedidoService.update(Number(id), { nomeCliente: data.nomeCliente, emailCliente: data.emailCliente, pago: data.pago });
            } else {
                await PedidoService.create({ nomeCliente: data.nomeCliente, emailCliente: data.emailCliente, itensPedido: data.itensPedido });
            }
            navigate('/pedidos');
        } catch (error) {
            setErro('Erro ao salvar pedido.');
            console.error('Erro ao salvar pedido:', error);
            setLoading(false);
        }
    };

    return (
        <div className="container mt-4" style={{ maxWidth: 700 }}>
            <h2>{isEdit ? 'Editar Pedido' : 'Novo Pedido'}</h2>
            {erro && <div className="alert alert-danger">{erro}</div>}
            <form onSubmit={handleSubmit(onSubmit)}>
                <div className="mb-3">
                    <label className="form-label">Nome do Cliente *</label>
                    <input className="form-control" {...register('nomeCliente', { required: 'Nome obrigatório', maxLength: { value: 60, message: 'Máx. 60 caracteres' } })} />
                    {errors.nomeCliente && <div className="text-danger small">{errors.nomeCliente.message}</div>}
                </div>
                <div className="mb-3">
                    <label className="form-label">Email do Cliente *</label>
                    <input type="email" className="form-control" {...register('emailCliente', { required: 'Email obrigatório', pattern: { value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/, message: 'Email inválido' } })} />
                    {errors.emailCliente && <div className="text-danger small">{errors.emailCliente.message}</div>}
                </div>
                {isEdit && (
                    <div className="mb-3 form-check">
                        <input type="checkbox" className="form-check-input" id="pago" {...register('pago')} />
                        <label className="form-check-label" htmlFor="pago">Pago</label>
                    </div>
                )}
                {!isEdit && (
                    <div>
                        <div className="d-flex justify-content-between align-items-center mb-2">
                            <h5>Itens do Pedido</h5>
                            <button type="button" className="btn btn-sm btn-outline-secondary" onClick={() => append({ idProduto: 0, quantidade: 1 })}>+ Item</button>
                        </div>
                        {fields.map((field, i) => (
                            <div key={field.id} className="row mb-2 align-items-end">
                                <div className="col-md-6">
                                    <label className="form-label">Produto *</label>
                                    <select className="form-select" {...register(`itensPedido.${i}.idProduto` as const, { required: 'Produto obrigatório', valueAsNumber: true, validate: v => v !== 0 || 'Produto obrigatório' })}>
                                        <option value={0} disabled>Selecione...</option>
                                        {produtos.map(p => <option key={p.id} value={p.id}>{p.nomeProduto} - {p.valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</option>)}
                                    </select>
                                    {errors.itensPedido && errors.itensPedido[i] && (errors.itensPedido[i] as any).idProduto && <div className="text-danger small">{(errors.itensPedido[i] as any).idProduto?.message}</div>}
                                </div>
                                <div className="col-md-4">
                                    <label className="form-label">Quantidade *</label>
                                    <input type="number" className="form-control" min={1} {...register(`itensPedido.${i}.quantidade` as const, { required: true, valueAsNumber: true, min: 1 })} />
                                </div>
                                <div className="col-md-2">
                                    <button type="button" className="btn btn-outline-danger w-100" onClick={() => remove(i)}>Remover</button>
                                </div>
                            </div>
                        ))}
                    </div>
                )}
                <div className="mt-3">
                    <button type="submit" className="btn btn-success me-2" disabled={loading}>Salvar</button>
                    <button type="button" className="btn btn-secondary" onClick={() => navigate('/pedidos')}>Cancelar</button>
                </div>
            </form>
        </div>
    );
}
