import api from './api';
import { Produto, CreateProduto, UpdateProduto } from '../models/Produto';

export const ProdutoService = {
    getAll: () => api.get<Produto[]>('/produtos').then(r => r.data),
    getById: (id: number) => api.get<Produto>(`/produtos/${id}`).then(r => r.data),
    create: (data: CreateProduto) => api.post<Produto>('/produtos', data).then(r => r.data),
    update: (id: number, data: UpdateProduto) => api.put<Produto>(`/produtos/${id}`, data).then(r => r.data),
    delete: (id: number) => api.delete(`/produtos/${id}`)
};
