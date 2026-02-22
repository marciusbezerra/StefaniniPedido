import { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { CreateProduto } from '../models/Produto';
import { ProdutoService } from '../services/produtoService';

export function ProdutoForm() {
    const { id } = useParams<{ id: string }>();
    const isEdit = Boolean(id);
    const navigate = useNavigate();
    const { register, handleSubmit, reset, formState: { errors, isSubmitting } } = useForm<CreateProduto>({
        defaultValues: { nomeProduto: '', valor: 0 }
    });

    useEffect(() => {
        if (isEdit && id) {
            ProdutoService.getById(Number(id)).then(p => reset(p));
        }
    }, [id]);

    const onSubmit = async (data: CreateProduto) => {
        if (isEdit) {
            await ProdutoService.update(Number(id), data);
        } else {
            await ProdutoService.create(data);
        }
        navigate('/produtos');
    };

    return (
        <div className="container mt-4" style={{ maxWidth: 500 }}>
            <h2>{isEdit ? 'Editar Produto' : 'Novo Produto'}</h2>
            <form onSubmit={handleSubmit(onSubmit)}>
                <div className="mb-3">
                    <label className="form-label">Nome do Produto *</label>
                    <input className="form-control" {...register('nomeProduto', { required: 'Nome obrigatório', maxLength: { value: 20, message: 'Máx. 20 caracteres' } })} />
                    {errors.nomeProduto && <div className="text-danger small">{errors.nomeProduto.message}</div>}
                </div>
                <div className="mb-3">
                    <label className="form-label">Valor (R$) *</label>
                    <input type="number" step="0.01" min="0.01" className="form-control"
                        {...register('valor', { required: 'Valor obrigatório', valueAsNumber: true, min: { value: 0.01, message: 'Valor deve ser maior que zero' } })} />
                    {errors.valor && <div className="text-danger small">{errors.valor.message}</div>}
                </div>
                <div className="mt-3">
                    <button type="submit" className="btn btn-success me-2" disabled={isSubmitting}>Salvar</button>
                    <button type="button" className="btn btn-secondary" onClick={() => navigate('/produtos')}>Cancelar</button>
                </div>
            </form>
        </div>
    );
}
