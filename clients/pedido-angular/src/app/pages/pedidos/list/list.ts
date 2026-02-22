import { Component, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Pedido } from '../../../core/models/pedido.model';
import { PedidoService } from '../../../core/services/pedido.service';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-pedidos-list',
  standalone: false,
  templateUrl: './list.html',
  styleUrl: './list.scss',
})
export class List implements OnInit {
  pedidos = signal<Pedido[]>([]);
  loading = signal(false);
  erro = signal('');

  constructor(private pedidoService: PedidoService, private router: Router) { }

  async ngOnInit(): Promise<void> {
    await this.carregar();
  }

  async carregar(): Promise<void> {
    this.loading.set(true);
    try {
      const data = await lastValueFrom(this.pedidoService.getAll());
      this.pedidos.set(data);
    } catch (error) {
      console.error('Erro ao carregar pedidos:', error);
      this.erro.set('Erro ao carregar pedidos.');
    } finally {
      this.loading.set(false);
    }
  }

  novo(): void {
    this.router.navigate(['/pedidos/novo']);
  }

  editar(id: number): void {
    this.router.navigate(['/pedidos/editar', id]);
  }

  verPedido(id: number): void {
    this.router.navigate(['/pedidos/ver', id]);
  }

  async deletar(id: number): Promise<void> {
    if (!confirm('Confirma exclusão do pedido?')) return;
    try {
      await lastValueFrom(this.pedidoService.delete(id));
      const updated = this.pedidos().filter(p => p.id !== id);
      this.pedidos.set(updated);
    } catch (error) {
      console.error('Erro ao deletar pedido:', error);
      this.erro.set('Erro ao deletar pedido.');
    }
  }
}
