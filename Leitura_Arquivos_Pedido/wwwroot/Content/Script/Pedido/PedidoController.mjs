import { Pedido } from "./PedidoDao.mjs";
const pedidoDao = new Pedido();
window.arrPedidos = new Array();

window.carregarPedidos = async () => {
    try {
        NProgress.configure({ showSpinner: false });
        NProgress.start();
        let pedido = arrPedidos[1];
        await pedidoDao.updatePedido(pedido.PedCod, pedido.CNPJ);
        NProgress.done();
    } catch (error) {
        console.error("Erro na atualização:", error);
    }
}

fetch("http://app.local/pedidos.json")
    .then(resp => resp.json())
    .then(pedidos => {
        arrPedidos = pedidos;
        const tbody = document.querySelector("#tabelaPedidos tbody");
        pedidos.forEach(p => {
            const row = document.createElement("tr");
            row.classList.add("odd:bg-white", "odd:dark:bg-gray-900", "even:bg-gray-50", "even:dark:bg-gray-800", "border-b", "dark:border-gray-700", "border-gray-200")
            row.innerHTML = `
                              <th scope="row" class="px-6 py-4 font-medium ">
                                ${p.PedCod}
                              </th>
                              <td class="px-6 py-4">
                                ${p.CNPJ}
                              </td>
                              <td class="px-6 py-4">
                                ${p.DataPedido}
                              </td>
                            `;
            tbody.appendChild(row);
        });
    })
    .catch(err => console.error(err));