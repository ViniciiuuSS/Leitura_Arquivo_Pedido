export class Pedido {
    async updatePedido(id, CNPJ) {
        const pedido = {
            CNPJ: CNPJ,
        };

        try {
            const response = await fetch(`http://localhost:5122/Pedido/${id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(pedido)
            });

            if (!response.ok) {
                throw new Error(`Erro: ${response.statusText}`);
            }

            console.log("Pedido atualizado com sucesso!");
            window.location.reload();
        } catch (error) {
            console.error("Falha ao atualizar o pedido:", error);
        }
    }
}