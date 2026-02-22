import api from './api';
import { Pedido, CreatePedido, UpdatePedido } from '../models/Pedido';

export const PedidoService = {
    getAll: () => api.get<Pedido[]>('/pedidos').then(r => r.data),
    getById: (id: number) => api.get<Pedido>(`/pedidos/${id}`).then(r => r.data),
    create: (data: CreatePedido) => api.post<Pedido>('/pedidos', data).then(r => r.data),
    update: (id: number, data: UpdatePedido) => api.put<Pedido>(`/pedidos/${id}`, data).then(r => r.data),
    delete: (id: number) => api.delete(`/pedidos/${id}`)
};
