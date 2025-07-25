export class Pedido {
    async updatePedido(pedcod, objPedido) {
        const id = pedcod;
        const pedido = objPedido.get(id);

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
            PNotify.success({ text: 'Dados Alterados com sucesso!' });
        } catch (error) {
            PNotify.error({ title: error })
            console.error("Falha ao atualizar o pedido:", error);
        }
    }
    async carregaProdutosPedido(pedcod, tbody) {
        try {
            const response = await fetch(`http://localhost:5122/Pedido/${pedcod}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error(`Erro: ${response.statusText}`);
            }

            const produtos = await response.json();
            
            $(tbody).empty();
            produtos.forEach(p => {
                const row = document.createElement("tr");
                row.setAttribute("pedcod", p.pedCod);
                row.classList.add(
                    "border-b", "border-gray-200",
                    (p.peCod % 2 === 0) ? "bg-gray-50" : "bg-white"
                );

                row.innerHTML = `
                        <th scope="row" class="px-6 py-4 font-medium text-gray-900">${p.pedCod}</th>
                        <td class="px-6 py-4 text-gray-900">
                          ${p.ean}
                        </td>
                        <td class="px-6 py-4 text-gray-900">
                            <div class="relative flex items-center max-w-[8rem]">
                                <input
                                  ean="${p.ean}"
                                  type="text"
                                  value="${p.quantidade}"
                                  aria-describedby="helper-text-explanation"
                                  class="bg-white border border-gray-300 h-11 text-center text-gray-900 text-sm focus:ring-blue-500 focus:border-blue-500 block w-full py-2.5 placeholder-gray-500"
                                  placeholder="999"
                                  required
                                  disabled
                                />

                            </div>
                        </td>
                      `;

                tbody.appendChild(row);
            });
        } catch (error) {
            PNotify.error({ title: error })
            console.error("Falha ao atualizar o pedido:", error);
        }
    }
    progress() {
        NProgress.configure({ showSpinner: false });
        NProgress.start();
    }
}