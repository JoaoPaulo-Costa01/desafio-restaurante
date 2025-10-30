import { useState, useEffect } from 'react';
import axios from 'axios';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  LineElement,
  PointElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import { Bar, Line } from 'react-chartjs-2';

// --- Registro ---
ChartJS.register(
  CategoryScale, LinearScale, BarElement, LineElement, PointElement,
  Title, Tooltip, Legend
);

// --- CSS Completo com Centralização e Labels ---
const styles = {
  // Container principal: Centralizado ('auto') e com largura máxima
  container: {
    fontFamily: 'Arial, sans-serif',
    width: '90%', // Usa 90% da largura disponível
    maxWidth: '1600px', // Limite máximo para telas muito largas
    margin: '30px auto', // Margem vertical + CENTRALIZAÇÃO HORIZONTAL ('auto')
    padding: '20px',
  },
  // Título principal
  header: {
    textAlign: 'center',
    color: '#FFFFFF',
    paddingBottom: '10px',
    marginBottom: '40px',
    fontSize: '2.5em'
  },
  // Container dos filtros: Centralizado
  filtersContainer: {
    display: 'flex',
    flexWrap: 'wrap',
    justifyContent: 'center', // Centraliza os grupos horizontalmente
    alignItems: 'flex-end', // Alinha a base dos elementos
    gap: '20px', // Espaço entre grupos
    marginBottom: '40px',
    padding: '25px',
    backgroundColor: 'rgba(255, 255, 255, 0.1)',
    borderRadius: '8px',
  },
  // Grupo Label + Input/Select
  filterGroup: {
    display: 'flex',
    flexDirection: 'column',
    gap: '5px',
    minWidth: '150px', // Largura mínima
    textAlign: 'center'
  },
  // Label do Filtro
  filterLabel: {
    fontSize: '0.85em',
    color: '#ccc',
    marginBottom: '3px',
    display: 'block'
  },
  // Estilo unificado inputs/selects
  filterControl: {
    padding: '10px 12px',
    fontSize: '1em',
    borderRadius: '5px',
    border: '1px solid #555',
    backgroundColor: '#333',
    color: '#eee',
    boxSizing: 'border-box',
    width: '100%'
  },
  // Input numérico
  inputNumber: {
    textAlign: 'center'
  },
  // Grid dos gráficos: USA FLEXBOX PARA CENTRALIZAR OS GRÁFICOS
  chartsGrid: {
    display: 'flex',
    flexDirection: 'column', // Empilha os gráficos
    alignItems: 'center',    // Centraliza eles horizontalmente
    gap: '30px',
    marginBottom: '40px',
  },
  // Caixa do gráfico: Altura grande
  chartBox: {
    padding: '25px',
    backgroundColor: '#f9f9ff',
    borderRadius: '10px',
    boxShadow: '0 6px 15px rgba(0,0,0,0.1)',
    height: '550px',
    width: '90%', // Gráfico tem 90% da largura...
    maxWidth: '1200px' // ...até um máximo de 1200px
  },
  // Container da Tabela:
  tableContainer: {
    backgroundColor: '#f9f9ff',
    borderRadius: '10px',
    boxShadow: '0 6px 15px rgba(0,0,0,0.1)',
    padding: '25px',
    marginTop: '40px',
    overflowX: 'auto',
    width: '90%', // Tabela também tem 90%...
    maxWidth: '1200px', // ...e o mesmo limite
    margin: '40px auto' // E se centraliza
  },
  // Título da Tabela
  tableTitle: {
    color: '#333',
    marginBottom: '20px',
    borderBottom: '1px solid #ddd',
    paddingBottom: '15px',
    fontSize: '1.5em'
  },
  // Tabela
  table: {
    width: '100%',
    borderCollapse: 'collapse',
  },
  // Cabeçalho da Tabela
  th: {
    borderBottom: '2px solid #ccc',
    padding: '15px 10px',
    textAlign: 'left',
    backgroundColor: '#f0f0f0',
    color: '#333',
    fontWeight: 'bold',
  },
  // Células da Tabela
  td: {
    borderBottom: '1px solid #eee',
    padding: '12px 10px',
    color: '#333'
  },
  // Linha "Nenhum dado"
  noDataRow: {
    textAlign: 'center',
    fontStyle: 'italic',
    color: '#777',
  }
};

// --- Listas para filtros ---
const channels = [
  { id: 1, name: "Presencial" }, { id: 2, name: "iFood" }, { id: 3, name: "Rappi" },
  { id: 4, name: "Uber Eats" }, { id: 5, name: "WhatsApp" }, { id: 6, name: "App Próprio" }
];
const daysOfWeek = [
  { id: 0, name: "Domingo" }, { id: 1, name: "Segunda-feira" }, { id: 2, name: "Terça-feira" },
  { id: 3, name: "Quarta-feira" }, { id: 4, name: "Quinta-feira" }, { id: 5, name: "Sexta-feira" }, { id: 6, name: "Sábado" }
];
const getPastDateString = (daysAgo) => {
  const date = new Date();
  date.setDate(date.getDate() - daysAgo);
  return date.toISOString().split('T')[0]; // Formato YYYY-MM-DD
};

// --- Cores para o Gráfico de Linhas ---
const lineChartColors = [
  { borderColor: 'rgba(53, 162, 235, 0.7)' }, // Azul
  { borderColor: 'rgba(255, 99, 132, 0.7)' },  // Vermelho
  { borderColor: 'rgba(75, 192, 192, 0.7)' },  // Verde
  { borderColor: 'rgba(153, 102, 255, 0.7)' }, // Roxo
  { borderColor: 'rgba(255, 159, 64, 0.7)' }, // Laranja
  { borderColor: 'rgba(255, 99, 255, 0.7)' }   // Rosa
];

// --- O Componente Principal do App ---
function App() {
  // --- Estados ---
  const [topProductsData, setTopProductsData] = useState({ labels: [], datasets: [] });
  const [topProductsTitle, setTopProductsTitle] = useState('Top 10 Produtos');
  const [selectedChannelId, setSelectedChannelId] = useState(2); // Canal inicial: iFood
  const [selectedDay, setSelectedDay] = useState(4); // Dia inicial: Quinta-feira
  const [ticketMedioData, setTicketMedioData] = useState({ labels: [], datasets: [] });
  const [deliveryTimeData, setDeliveryTimeData] = useState({ labels: [], datasets: [] });
  const [inactiveCustomers, setInactiveCustomers] = useState([]);
  const [minComprasFilter, setMinComprasFilter] = useState(3);
  const [diasInativoFilter, setDiasInativoFilter] = useState(30);
  const [dateRange, setDateRange] = useState({
    startDate: getPastDateString(30), // Padrão: Últimos 30 dias
    endDate: getPastDateString(0)     // Padrão: Hoje
  });
  

  // --- Efeito 1: Buscar Top Produtos ---
  useEffect(() => {
    const channelName = channels.find(c => c.id == selectedChannelId)?.name || '';
    const dayName = daysOfWeek.find(d => d.id == selectedDay)?.name || '';
    setTopProductsTitle(`Top 10 Produtos (${dayName} no ${channelName})`);
    const apiUrl = `https://localhost:7180/api/analytics/top-products?channelId=${selectedChannelId}&dayOfWeek=${selectedDay}`;
    axios.get(apiUrl)
      .then(response => {
        const apiData = response.data;
        setTopProductsData({
          labels: apiData.map(item => item.productName),
          datasets: [{ label: 'Total Vendido', data: apiData.map(item => item.totalSold), backgroundColor: 'rgba(53, 162, 235, 0.5)' }],
        });
      })
      .catch(error => console.error("Erro ao buscar Top Produtos:", error));
  }, [selectedChannelId, selectedDay]); // Depende do canal e dia selecionados

  // --- Efeito 2: Buscar Ticket Médio ---
  useEffect(() => {
    const { startDate, endDate } = dateRange;
    if (!startDate || !endDate) return; // Não busca se as datas forem inválidas
    const apiUrl = `https://localhost:7180/api/analytics/ticket-medio-por-canal?startDate=${startDate}&endDate=${endDate}`;
    axios.get(apiUrl)
      .then(response => {
        const apiData = response.data;
        const coloredDatasets = apiData.datasets.map((dataset, index) => ({
          ...dataset,
          borderColor: lineChartColors[index % lineChartColors.length].borderColor,
          borderWidth: 3,
          fill: false // Linha sem preenchimento
        }));
        setTicketMedioData({ labels: apiData.labels, datasets: coloredDatasets });
      })
      .catch(error => console.error("Erro ao buscar Ticket Médio:", error));
  }, [dateRange]); // Depende do período selecionado

  // --- Efeito 3: Buscar Tempo Médio de Entrega ---
  useEffect(() => {
    const { startDate, endDate } = dateRange;
    if (!startDate || !endDate) return;
    const apiUrl = `https://localhost:7180/api/analytics/tempo-medio-entrega?startDate=${startDate}&endDate=${endDate}`;
    axios.get(apiUrl)
      .then(response => {
        const apiData = response.data;
        setDeliveryTimeData({
          labels: apiData.labels,
          datasets: [{ label: 'Tempo Médio (min)', data: apiData.data, borderColor: 'rgb(255, 99, 132)', borderWidth: 3, fill: false, tension: 0.1 }]
        });
      })
      .catch(error => console.error("Erro ao buscar Tempo de Entrega:", error));
  }, [dateRange]); // Depende do período selecionado

  // --- Efeito 4: Buscar Clientes Inativos ---
  useEffect(() => {
    // Garante que os filtros numéricos sejam válidos
    if (minComprasFilter < 1 || diasInativoFilter < 1) return;
    const apiUrl = `https://localhost:7180/api/analytics/clientes-inativos?minCompras=${minComprasFilter}&diasInativo=${diasInativoFilter}`;
    axios.get(apiUrl)
      .then(response => {
        setInactiveCustomers(response.data);
      })
      .catch(error => {
        console.error("Erro ao buscar Clientes Inativos:", error);
        setInactiveCustomers([]); // Limpa em caso de erro
      });
  }, [minComprasFilter, diasInativoFilter]); // Depende dos filtros numéricos


  //
  // =======================================================================
  // --- EFEITO 5: CORREÇÃO DA CENTRALIZAÇÃO (O CÓDIGO NOVO ESTÁ AQUI) ---
  // =======================================================================
  useEffect(() => {
    // O template do Vite/React (index.css) define o body como 'display: flex'
    // e 'place-items: center', o que quebra o 'margin: auto' do nosso container.
    // Esta é uma correção forçada para remover esse comportamento.
    document.body.style.display = 'block';
    document.body.style.placeItems = 'initial';
    
    // Também vamos garantir o fundo escuro (que também vem do index.css)
    document.body.style.backgroundColor = '#242424';
  }, []); // O array vazio [] garante que isso rode apenas UMA VEZ.
  // =======================================================================
  //
  

  // --- Handlers para os filtros ---
  const handleChannelChange = (e) => setSelectedChannelId(e.target.value);
  const handleDayChange = (e) => setSelectedDay(e.target.value);
  const handleDateChange = (e) => {
    setDateRange(prevRange => ({
      ...prevRange,
      [e.target.name]: e.target.value
    }));
  };
  const handleMinComprasChange = (e) => {
    const value = parseInt(e.target.value, 10);
    setMinComprasFilter(value > 0 ? value : 1); // Garante valor positivo
  };
  const handleDiasInativoChange = (e) => {
    const value = parseInt(e.target.value, 10);
    setDiasInativoFilter(value > 0 ? value : 1); // Garante valor positivo
  };


  // --- JSX (Renderização com todas as Labels) ---
  return (
    <div style={styles.container}>
      <h1 style={styles.header}>Dashboard de Análise de Vendas</h1>

      {/* Filtros */}
      <div style={styles.filtersContainer}>
        {/* Filtros Gráficos */}
        <div style={styles.filterGroup}>
          <label style={styles.filterLabel} htmlFor="channelSelect">Canal</label>
          <select id="channelSelect" style={styles.filterControl} value={selectedChannelId} onChange={handleChannelChange}>
            {channels.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
          </select>
        </div>

        <div style={styles.filterGroup}>
          <label style={styles.filterLabel} htmlFor="daySelect">Dia da Semana</label>
          <select id="daySelect" style={styles.filterControl} value={selectedDay} onChange={handleDayChange}>
            {daysOfWeek.map(d => <option key={d.id} value={d.id}>{d.name}</option>)}
          </select>
        </div>

        <div style={styles.filterGroup}>
          <label style={styles.filterLabel} htmlFor="startDate">Data Início</label>
          <input id="startDate" style={styles.filterControl} type="date" name="startDate" value={dateRange.startDate} onChange={handleDateChange} title="Data Inicial"/>
        </div>

        <div style={styles.filterGroup}>
          <label style={styles.filterLabel} htmlFor="endDate">Data Final</label>
          <input id="endDate" style={styles.filterControl} type="date" name="endDate" value={dateRange.endDate} onChange={handleDateChange} title="Data Final"/>
        </div>

        {/* Filtros Tabela */}
        <div style={styles.filterGroup}>
          <label style={styles.filterLabel} htmlFor="minCompras">Min. Compras</label>
          <input
            id="minCompras"
            type="number" min="1" style={{...styles.filterControl, ...styles.inputNumber}}
            value={minComprasFilter} onChange={handleMinComprasChange}
            title="Mínimo de Compras"
          />
        </div>
        <div style={styles.filterGroup}>
          <label style={styles.filterLabel} htmlFor="diasInativo">Dias Inativo</label>
          <input
            id="diasInativo"
            type="number" min="1" style={{...styles.filterControl, ...styles.inputNumber}}
            value={diasInativoFilter} onChange={handleDiasInativoChange}
            title="Dias Inativo"
          />
        </div>
      </div>

      {/* Grid de Gráficos */}
      <div style={styles.chartsGrid}>
        {/* Gráfico 1: Top Produtos */}
        <div style={styles.chartBox}>
          <Bar options={{ responsive: true, maintainAspectRatio: false, plugins: { legend: { position: 'top' }, title: { display: true, text: topProductsTitle }}}} data={topProductsData} />
        </div>
        {/* Gráfico 2: Ticket Médio */}
        <div style={styles.chartBox}>
          <Line options={{ responsive: true, maintainAspectRatio: false, plugins: { legend: { position: 'top' }, title: { display: true, text: 'Ticket Médio por Canal' }}, scales: { y: { beginAtZero: true, ticks: { callback: (v) => 'R$ ' + v }}}}} data={ticketMedioData} />
        </div>
        {/* Gráfico 3: Tempo Médio de Entrega */}
        <div style={styles.chartBox}>
          <Line options={{ responsive: true, maintainAspectRatio: false, plugins: { legend: { position: 'top' }, title: { display: true, text: 'Tempo Médio de Entrega (Geral)' }}, scales: { y: { beginAtZero: false, ticks: { callback: (v) => v ? v.toFixed(1) + ' min' : '0 min' }}}}} data={deliveryTimeData} />
        </div>
      </div>

      {/* Tabela de Clientes Inativos */}
      <div style={styles.tableContainer}>
        <h2 style={styles.tableTitle}>
          Clientes Inativos ({minComprasFilter}+ compras, sem comprar há {diasInativoFilter}+ dias)
        </h2>
        <table style={styles.table}>
          <thead>
            <tr>
              <th style={styles.th}>Nome</th>
              <th style={styles.th}>Email</th>
              <th style={styles.th}>Telefone</th>
              <th style={styles.th}>Total Compras</th>
              <th style={styles.th}>Última Compra</th>
            </tr>
          </thead>
          <tbody>
            {inactiveCustomers.length > 0 ? (
              inactiveCustomers.map(customer => (
                <tr key={customer.customerId}>
                  <td style={styles.td}>{customer.customerName || 'N/A'}</td>
                  <td style={styles.td}>{customer.email || 'N/A'}</td>
                  <td style={styles.td}>{customer.phoneNumber || 'N/A'}</td>
                  <td style={styles.td}>{customer.totalCompras}</td>
                  <td style={styles.td}>{customer.ultimaCompra ? new Date(customer.ultimaCompra).toLocaleDateString() : 'N/A'}</td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="5" style={{...styles.td, ...styles.noDataRow}}>
                  Nenhum cliente encontrado para estes filtros.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

    </div>
  );
}

export default App;