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
import './App.css'; // Importa o CSS separado

ChartJS.register(
  CategoryScale, LinearScale, BarElement, LineElement, PointElement,
  Title, Tooltip, Legend
);

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
  return date.toISOString().split('T')[0];
};
const lineChartColors = [
  { borderColor: 'rgba(53, 162, 235, 0.7)' },
  { borderColor: 'rgba(255, 99, 132, 0.7)' },
  { borderColor: 'rgba(75, 192, 192, 0.7)' },
  { borderColor: 'rgba(153, 102, 255, 0.7)' },
  { borderColor: 'rgba(255, 159, 64, 0.7)' },
  { borderColor: 'rgba(255, 99, 255, 0.7)' }
];

function App() {
  const [topProductsData, setTopProductsData] = useState({ labels: [], datasets: [] });
  const [topProductsTitle, setTopProductsTitle] = useState('Top 10 Produtos');
  const [selectedChannelId, setSelectedChannelId] = useState(2);
  const [selectedDay, setSelectedDay] = useState(4);
  const [ticketMedioData, setTicketMedioData] = useState({ labels: [], datasets: [] });
  const [deliveryTimeData, setDeliveryTimeData] = useState({ labels: [], datasets: [] });
  const [inactiveCustomers, setInactiveCustomers] = useState([]);
  const [minComprasFilter, setMinComprasFilter] = useState(3);
  const [diasInativoFilter, setDiasInativoFilter] = useState(30);
  const [dateRange, setDateRange] = useState({
    startDate: getPastDateString(30),
    endDate: getPastDateString(0)
  });

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
  }, [selectedChannelId, selectedDay]);

  useEffect(() => {
    const { startDate, endDate } = dateRange;
    if (!startDate || !endDate) return;
    const apiUrl = `https://localhost:7180/api/analytics/ticket-medio-por-canal?startDate=${startDate}&endDate=${endDate}`;
    axios.get(apiUrl)
      .then(response => {
        const apiData = response.data;
        const coloredDatasets = apiData.datasets.map((dataset, index) => ({
          ...dataset,
          borderColor: lineChartColors[index % lineChartColors.length].borderColor,
          borderWidth: 3,
          fill: false
        }));
        setTicketMedioData({ labels: apiData.labels, datasets: coloredDatasets });
      })
      .catch(error => console.error("Erro ao buscar Ticket Médio:", error));
  }, [dateRange]);

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
  }, [dateRange]);

  useEffect(() => {
    if (minComprasFilter < 1 || diasInativoFilter < 1) return;
    const apiUrl = `https://localhost:7180/api/analytics/clientes-inativos?minCompras=${minComprasFilter}&diasInativo=${diasInativoFilter}`;
    axios.get(apiUrl)
      .then(response => {
        setInactiveCustomers(response.data);
      })
      .catch(error => {
          console.error("Erro ao buscar Clientes Inativos:", error);
          setInactiveCustomers([]);
      });
  }, [minComprasFilter, diasInativoFilter]);

  useEffect(() => {
    document.body.style.display = 'block';
    document.body.style.placeItems = 'initial';
    document.body.style.backgroundColor = '#242424';
  }, []);

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
    setMinComprasFilter(value > 0 ? value : 1);
  };
  const handleDiasInativoChange = (e) => {
    const value = parseInt(e.target.value, 10);
    setDiasInativoFilter(value > 0 ? value : 1);
  };

  return (
    <div className="container">
      <h1 className="header">Dashboard de Análise de Vendas</h1>

      <div className="filtersContainer">
        <div className="filterGroup">
          <label className="filterLabel" htmlFor="channelSelect">Canal</label>
          <select id="channelSelect" className="filterControl" value={selectedChannelId} onChange={handleChannelChange}>
            {channels.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
          </select>
        </div>

        <div className="filterGroup">
          <label className="filterLabel" htmlFor="daySelect">Dia da Semana</label>
          <select id="daySelect" className="filterControl" value={selectedDay} onChange={handleDayChange}>
            {daysOfWeek.map(d => <option key={d.id} value={d.id}>{d.name}</option>)}
          </select>
        </div>

        <div className="filterGroup">
          <label className="filterLabel" htmlFor="startDate">Data Início</label>
          <input id="startDate" className="filterControl" type="date" name="startDate" value={dateRange.startDate} onChange={handleDateChange} title="Data Inicial"/>
        </div>

        <div className="filterGroup">
          <label className="filterLabel" htmlFor="endDate">Data Final</label>
          <input id="endDate" className="filterControl" type="date" name="endDate" value={dateRange.endDate} onChange={handleDateChange} title="Data Final"/>
        </div>

        <div className="filterGroup">
          <label className="filterLabel" htmlFor="minCompras">Min. Compras</label>
          <input
              id="minCompras"
              type="number" min="1" className="filterControl inputNumber"
              value={minComprasFilter} onChange={handleMinComprasChange}
              title="Mínimo de Compras"
           />
        </div>
        <div className="filterGroup">
          <label className="filterLabel" htmlFor="diasInativo">Dias Inativo</label>
          <input
              id="diasInativo"
              type="number" min="1" className="filterControl inputNumber"
              value={diasInativoFilter} onChange={handleDiasInativoChange}
              title="Dias Inativo"
           />
        </div>
      </div>

      <div className="chartsGrid">
        <div className="chartBox">
          <Bar options={{ responsive: true, maintainAspectRatio: false, plugins: { legend: { position: 'top' }, title: { display: true, text: topProductsTitle }}}} data={topProductsData} />
        </div>
        <div className="chartBox">
          <Line options={{ responsive: true, maintainAspectRatio: false, plugins: { legend: { position: 'top' }, title: { display: true, text: 'Ticket Médio por Canal' }}, scales: { y: { beginAtZero: true, ticks: { callback: (v) => 'R$ ' + v }}}}} data={ticketMedioData} />
        </div>
        <div className="chartBox">
          <Line options={{ responsive: true, maintainAspectRatio: false, plugins: { legend: { position: 'top' }, title: { display: true, text: 'Tempo Médio de Entrega (Geral)' }}, scales: { y: { beginAtZero: false, ticks: { callback: (v) => v ? v.toFixed(1) + ' min' : '0 min' }}}}} data={deliveryTimeData} />
        </div>
      </div>

      <div className="tableContainer">
        <h2 className="tableTitle">
            Clientes Inativos ({minComprasFilter}+ compras, sem comprar há {diasInativoFilter}+ dias)
        </h2>
        <table className="table">
          <thead>
            <tr>
              <th className="th">Nome</th>
              <th className="th">Email</th>
              <th className="th">Telefone</th>
              <th className="th">Total Compras</th>
              <th className="th">Última Compra</th>
            </tr>
          </thead>
          <tbody>
            {inactiveCustomers.length > 0 ? (
              inactiveCustomers.map(customer => (
                <tr key={customer.customerId}>
                  <td className="td">{customer.customerName || 'N/A'}</td>
                  <td className="td">{customer.email || 'N/A'}</td>
                  <td className="td">{customer.phoneNumber || 'N/A'}</td>
                  <td className="td">{customer.totalCompras}</td>
                  <td className="td">{customer.ultimaCompra ? new Date(customer.ultimaCompra).toLocaleDateString() : 'N/A'}</td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="5" className="td noDataRow">
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