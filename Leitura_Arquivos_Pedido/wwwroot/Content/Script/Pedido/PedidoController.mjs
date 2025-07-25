import { Pedido } from "./PedidoDao.mjs";
const pedidoDao = new Pedido();
window.arrPedidos = new Array();
window.inputOpen = undefined;
window.edicao = false;
window.pedidoDetalheAberto = undefined;

window._init = () => {
    pedidoDao.progress();
    fetch("http://app.local/pedidos.json")
        .then(resp => resp.json())
        .then(pedidos => {
            arrPedidos = pedidos;
            const tbody = document.querySelector("#tabelaPedidos tbody");

            pedidos.forEach(p => {
                const row = document.createElement("tr");
                row.setAttribute("pedcod", p.PedCod);
                row.classList.add(
                    "border-b", "border-gray-200",
                    (p.PedCod % 2 === 0) ? "bg-gray-50" : "bg-white"
                );

                row.innerHTML = `
                <th scope="row" class="px-6 py-4 font-medium text-gray-900">${p.PedCod}</th>
                <td class="px-6 py-4 text-gray-900">
                  ${p.CNPJ}
                </td>
                <td class="px-6 py-4 text-gray-900">${p.DataPedido}</td>
                <td class="px-6 py-4">
                  <button type="button" data-modal-target="default-modal" data-modal-toggle="default-modal"
                    class="detalhes text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5">
                    Detalhes
                  </button>
                </td>
              `;

                tbody.appendChild(row);
            });

            configuraAmbiente?.();
            NProgress.done();
        })
        .catch(err => console.error(err));

    function configuraAmbiente() {
        $(".detalhes").on("click", async (e) => {
            const tbody = document.querySelector("#tabelaPedidos_Produtos tbody");
            const tr = e.target.closest("tr");
            const pedcod = tr.getAttribute("pedcod");
            document.getElementsByClassName("btnEditar").innerText = "Editar"
            tbody.innerHTML = geraSpinner();
            pedidoDao.progress();
            pedidoDetalheAberto = pedcod;
            elFoco(tr);
            number_PedCod.innerHTML = pedcod;
            await pedidoDao.carregaProdutosPedido(pedcod, tbody);
            NProgress.done();
        });
        $(".btnEditar").on("click", async (e) => {
            const arrIputs = Array.from(document.querySelectorAll("#tabelaPedidos_Produtos tbody input"));
            if (!edicao) {
                edicao = true;
                e.target.innerText = "Alterar";
                configuraEdicaoModal(arrIputs, false);
                return;
            }
            let qtdProdutos = new Map();
            let pedcod = undefined;
            let objProd = new Array();
            arrIputs.forEach(input => {
                if (!pedcod) {
                    const tr = input.closest("tr");
                    pedcod = tr.getAttribute("pedcod");
                }
                const ean = input.getAttribute("ean")
                objProd.push({ EAN: ean, Quantidade: +input.value });
            });
            qtdProdutos.set(pedcod, objProd);
            pedidoDao.progress();
            await pedidoDao.updatePedido(pedcod, qtdProdutos);
            NProgress.done();
            edicao = false;
            e.target.innerText = "Editar";
            configuraEdicaoModal(arrIputs, true);
        });
        function configuraEdicaoModal(arrIputs, status) {
            arrIputs.forEach(input => {
                input.disabled = status;
            });
        }
        initModals();
        configurarPesquisa("tabelaPedidos");
    }
    function elFoco(elemento) {
        if (!elemento) return;

        elemento.focus();

        const offset = elemento.getBoundingClientRect().top + window.scrollY;
        const alturaTela = window.innerHeight;

        window.scrollTo({
            top: offset - alturaTela / 2 + elemento.offsetHeight / 2,
            behavior: "smooth"
        });
    }
    function geraSpinner() {
        return `<div class="text-center">
                    <div role="status">
                        <svg aria-hidden="true" class="inline w-8 h-8 text-gray-200 animate-spin dark:text-gray-600 fill-blue-600" viewBox="0 0 100 101" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <path d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z" fill="currentColor"/>
                            <path d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z" fill="currentFill"/>
                        </svg>
                        <span class="sr-only">Loading...</span>
                    </div>
                </div>`
    }
    function configurarPesquisa(tableId) {
        const input = document.getElementById('Filtro');
        const table = document.getElementById(tableId);

        if (!input || !table) return;

        input.addEventListener('input', function () {
            const filter = input.value.toLowerCase().trim();
            const rows = table.querySelectorAll('tbody tr');

            rows.forEach(row => {
                const cells = Array.from(row.querySelectorAll('td'));

                // Remove �ltima c�lula (geralmente "Detalhes", com bot�o ou a��es)
                const searchCells = cells.slice(0, -1);

                const rowText = searchCells.map(cell => cell.textContent.toLowerCase()).join(' ');
                const shouldShow = filter === '' || rowText.includes(filter);
                row.style.display = shouldShow ? '' : 'none';
            });
        });

        document.addEventListener('keydown', function (event) {
            if (event.ctrlKey && event.key === 'f') {
                event.preventDefault();
                input.focus();
            }
        });
    }

}

_init();