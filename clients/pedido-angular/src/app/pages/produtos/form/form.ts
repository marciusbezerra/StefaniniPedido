import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProdutoService } from '../../../core/services/produto.service';
import { lastValueFrom } from 'rxjs';
import { form, maxLength, min, required } from '@angular/forms/signals';
import { CreateProduto } from '../../../core/models/produto.model';

@Component({
  selector: 'app-produtos-form',
  standalone: false,
  templateUrl: './form.html',
  styleUrl: './form.scss',
})
export class Form implements OnInit {

  isEdit = signal(false);
  produtoId?: number;
  loading = signal(false);
  erro = signal('');

  produtoModel = signal<CreateProduto>({
    nomeProduto: '',
    valor: 0
  });

  produtoForm = form(this.produtoModel, (schemaPath) => {
    required(schemaPath.nomeProduto, { message: 'Nome do produto é obrigatório.' });
    maxLength(schemaPath.nomeProduto, 20, { message: 'Nome do produto deve ter no máximo 20 caracteres.' });
    required(schemaPath.valor, { message: 'Valor é obrigatório.' });
    min(schemaPath.valor, 0.01, { message: 'Valor deve ser maior que zero.' });

  });

  constructor(
    private produtoService: ProdutoService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  async ngOnInit(): Promise<void> {

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit.set(true);
      this.produtoId = +id;
      const data = await lastValueFrom(this.produtoService.getById(this.produtoId));
      this.produtoModel.set({ nomeProduto: data.nomeProduto, valor: data.valor });
    }
  }

  async salvar(): Promise<void> {
    if (this.produtoForm().invalid()) return;
    this.loading.set(true);
    try {
      const val = this.produtoModel();
      const obs = this.isEdit()
        ? this.produtoService.update(this.produtoId!, val)
        : this.produtoService.create(val);
      await lastValueFrom(obs);
      this.router.navigate(['/produtos']);
    } catch (error) {
      console.error('Erro ao salvar produto:', error);
      this.erro.set('Erro ao salvar produto.');
    } finally {
      this.loading.set(false);
    }
  }

  cancelar(): void {
    this.router.navigate(['/produtos']);
  }

  showErrors(field: any): string {
    const errors = field.errors();
    if (!errors) return '';
    return Object.values(errors).map((e: any) => e.message).join(', ');
  }
}
