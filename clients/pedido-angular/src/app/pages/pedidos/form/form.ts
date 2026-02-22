import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PedidoService } from '../../../core/services/pedido.service';
import { ProdutoService } from '../../../core/services/produto.service';
import { Produto } from '../../../core/models/produto.model';
import { applyEach, email, form, maxLength, required } from '@angular/forms/signals';
import { lastValueFrom } from 'rxjs';
import { PedidoForm } from '../../../core/models/pedido.model';

@Component({
  selector: 'app-pedidos-form',
  standalone: false,
  templateUrl: './form.html',
  styleUrl: './form.scss',
})
export class Form implements OnInit {

  produtos = signal<Produto[]>([]);
  isEdit = signal(false);
  pedidoId = signal<number | null>(null);
  loading = signal(false);
  erro = signal('');

  pedidoModel = signal<PedidoForm>({
    nomeCliente: '',
    emailCliente: '',
    pago: false,
    itensPedido: []
  });

  pedidoForm = form(this.pedidoModel, (schemaPath) => {
    required(schemaPath.nomeCliente, { message: 'Nome do cliente é obrigatório.' });
    maxLength(schemaPath.nomeCliente, 50, { message: 'Nome do cliente deve ter no máximo 50 caracteres.' });
    required(schemaPath.emailCliente, { message: 'Email do cliente é obrigatório.' });
    email(schemaPath.emailCliente, { message: 'Email do cliente deve ser válido.' });
    applyEach(schemaPath.itensPedido, (f) => {
      required(f.idProduto, { message: 'Produto é obrigatório' });
      required(f.quantidade, { message: 'Quantidade é obrigatória' });
    });
  });

  constructor(
    private pedidoService: PedidoService,
    private produtoService: ProdutoService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  async ngOnInit(): Promise<void> {
    try {
      this.produtos.set(await lastValueFrom(this.produtoService.getAll()));
      const id = this.route.snapshot.paramMap.get('id');
      if (id) {
        this.isEdit.set(true);
        this.pedidoId?.set(+id);
        const pedidoIdValue = this.pedidoId();
        if (pedidoIdValue !== null) {
          const data = await lastValueFrom(this.pedidoService.getById(pedidoIdValue));
          this.pedidoModel.set({ nomeCliente: data.nomeCliente, emailCliente: data.emailCliente, pago: data.pago, itensPedido: data.itensPedido.map(i => ({ idProduto: String(i.idProduto), quantidade: i.quantidade })) });
        }
      } else {
        this.addItem();
      }
    } catch (error) {
      console.error('Erro ao carregar produtos ou pedido:', error);
      this.erro.set('Erro ao carregar produtos ou pedido.');
    }
  }

  addItem(): void {
    this.pedidoModel.set({
      ...this.pedidoModel(),
      itensPedido: [...this.pedidoModel().itensPedido, { idProduto: '', quantidade: 1 }]
    });
  }

  removeItem(i: number): void {
    const itens = [...this.pedidoModel().itensPedido];
    itens.splice(i, 1);
    this.pedidoModel.set({
      ...this.pedidoModel(),
      itensPedido: itens
    });
  }

  async salvar(): Promise<void> {
    if (this.pedidoForm().invalid()) return;
    this.loading.set(true);
    try {
      const val = this.pedidoModel();
      if (this.isEdit()) {
        const pedidoIdValue = this.pedidoId();
        if (pedidoIdValue !== null) {

          await lastValueFrom(this.pedidoService.update(pedidoIdValue, {
            nomeCliente: val.nomeCliente,
            emailCliente: val.emailCliente,
            pago: val.pago,
          }));

          this.router.navigate(['/pedidos'])
        }
      } else {

        await lastValueFrom(this.pedidoService.create({
          nomeCliente: val.nomeCliente,
          emailCliente: val.emailCliente,
          itensPedido: val.itensPedido.map(i => ({ idProduto: Number(i.idProduto || 0), quantidade: i.quantidade }))
        }));

        this.router.navigate(['/pedidos']);
      }
    } catch (error) {
      console.error('Erro ao salvar pedido:', error);
      this.erro.set('Erro ao salvar pedido.');
    } finally {
      this.loading.set(false);
    }
  }

  cancelar(): void {
    this.router.navigate(['/pedidos']);
  }

  showErrors(field: any): string {
    const errors = field.errors();
    if (!errors) return '';
    return Object.values(errors).map((e: any) => e.message).join(', ');
  }
}
