import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Pedido, CreatePedido, UpdatePedido } from '../models/pedido.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class PedidoService {
  private readonly apiUrl = `${environment.apiUrl}/pedidos`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<Pedido[]> {
    return this.http.get<Pedido[]>(this.apiUrl);
  }

  getById(id: number): Observable<Pedido> {
    return this.http.get<Pedido>(`${this.apiUrl}/${id}`);
  }

  create(pedido: CreatePedido): Observable<Pedido> {
    return this.http.post<Pedido>(this.apiUrl, pedido);
  }

  update(id: number, pedido: UpdatePedido): Observable<Pedido> {
    return this.http.put<Pedido>(`${this.apiUrl}/${id}`, pedido);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
