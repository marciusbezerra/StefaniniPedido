import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PedidoService } from '../../../core/services/pedido.service';
import { lastValueFrom } from 'rxjs';
import { Pedido } from '../../../core/models/pedido.model';

@Component({
  selector: 'app-pedidos-ver',
  standalone: false,
  templateUrl: './ver.html',
  styleUrl: './ver.scss',
})
export class Ver implements OnInit {

  pedidoId: number | null = null;
  loading = signal(false);
  erro = signal('');
  pedido = signal<Pedido | null>(null);

  constructor(
    private pedidoService: PedidoService,
    private route: ActivatedRoute,
  ) { }

  async ngOnInit(): Promise<void> {
    try {
      const id = this.route.snapshot.paramMap.get('id');
      if (id) {
        this.pedidoId = +id;
        if (this.pedidoId) {
          this.pedido.set(await lastValueFrom(this.pedidoService.getById(this.pedidoId)));
          console.log('Pedido carregado:', this.pedido());
        }
      }
    } catch (error) {
      console.error('Erro ao carregar pedido:', error);
      this.erro.set('Erro ao carregar pedido.');
    }
  }
}
