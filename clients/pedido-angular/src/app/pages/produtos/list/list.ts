import { Component, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Produto } from '../../../core/models/produto.model';
import { ProdutoService } from '../../../core/services/produto.service';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-produtos-list',
  standalone: false,
  templateUrl: './list.html',
  styleUrl: './list.scss'
})

export class List implements OnInit {
  produtos = signal<Produto[]>([]);
  loading = signal(false);
  erro = signal('');

  constructor(private produtoService: ProdutoService, private router: Router) { }

  ngOnInit(): void { this.carregar(); }

  async carregar(): Promise<void> {
    this.loading.set(true);
    try {
      const data = await lastValueFrom(this.produtoService.getAll());
      this.produtos.set(data);
    } catch (error) {
      console.error('Erro ao carregar produtos:', error);
      this.erro.set('Erro ao carregar produtos.');
    } finally {
      this.loading.set(false);
    }
  }

  novo(): void {
    this.router.navigate(['/produtos/novo']);
  }
  editar(id: number): void {
    this.router.navigate(['/produtos/editar', id]);
  }

  async deletar(id: number): Promise<void> {
    if (!confirm('Confirma exclusão do produto?')) return;
    try {
      await lastValueFrom(this.produtoService.delete(id));
      const updated = this.produtos().filter(p => p.id !== id);
      this.produtos.set(updated);
    } catch (error) {
      console.error('Erro ao deletar produto:', error);
      this.erro.set('Erro ao deletar produto.');
    }
  }
}
