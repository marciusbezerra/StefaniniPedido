export interface ItensPedido {
    id: number;
    idProduto: number;
    nomeProduto: string;
    valorUnitario: number;
    quantidade: number;
}

export interface Pedido {
    id: number;
    nomeCliente: string;
    emailCliente: string;
    pago: boolean;
    valorTotal: number;
    itensPedido: ItensPedido[];
}

export interface CreateItensPedido {
    idProduto: number;
    quantidade: number;
}

export interface CreatePedido {
    nomeCliente: string;
    emailCliente: string;
    itensPedido: CreateItensPedido[];
}

export interface UpdatePedido {
    nomeCliente: string;
    emailCliente: string;
    pago: boolean;
}
