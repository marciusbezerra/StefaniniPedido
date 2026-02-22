export interface Produto {
    id: number;
    nomeProduto: string;
    valor: number;
}

export interface CreateProduto {
    nomeProduto: string;
    valor: number;
}

export interface UpdateProduto {
    nomeProduto: string;
    valor: number;
}