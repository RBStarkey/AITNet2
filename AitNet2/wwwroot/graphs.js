// This modeule is outside the js folder because TS keeps deleting it
window.chartColors = {
    red: 'rgb(255, 0, 0)',
    orange: 'rgb(255, 159, 64)',
    yellow: 'rgb(255, 205, 86)',
    green: 'rgb(75, 192, 192)',
    blue: 'rgb(54, 162, 235)',
    purple: 'rgb(153, 102, 255)',
    grey: 'rgb(201, 203, 207)'
};

function getRandom() { // min and max included
    var max = 100;
    var min = -100;
    return Math.floor(Math.random() * (max - min + 1) + min);
};
var alphaData = [getRandom(), getRandom(), getRandom(), getRandom(), getRandom(), getRandom(), getRandom()];
var betaData = [getRandom(), getRandom(), getRandom(), getRandom(), getRandom(), getRandom(), getRandom()];
var controlData = [getRandom(), getRandom(), getRandom(), getRandom(), getRandom(), getRandom(), getRandom()];
var graphData = [12, 19, 3, 12, 2, 3, 5, 20.8];

function showCharts() {

    var ctxBar = document.getElementById('barGraph').getContext('2d');
    barConfig = {
        type: 'bar',
        data: {
            labels: ['Red', 'Blue', 'Yellow', 'Light Blue', 'Purple', 'Orange', 'Aquamarine', 'Green'],
            datasets: [{
                label: '# of Votes',
                data: graphData,
                backgroundColor: [
                    'rgba(255, 0, 0, 0.2)',
                    'rgba(10, 10, 170, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)',
                    'rgba(127, 255, 212, 0.2)',
                    'rgba(123, 456, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 0, 0, 1)',
                    'rgba(10, 10, 170, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)',
                    'rgba(127, 255, 212, 1)',
                    'rgba(123, 255, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            title: {
                display: true,
                text: 'Bar Chart Showing Returns From Suppliers During June 2020'
            },
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            },
            legend: false
        }
    }
    window.barChart = new Chart(ctxBar, barConfig);

    var ctxThreeLine = document.getElementById('threeLineGraph').getContext('2d');
    var lineConfig = {
        type: 'line',
        data: {
            labels: ['January', 'February', 'March', 'April', 'May', 'June'],
            datasets: [{
                label: 'Alpha',
                fill: false,
                backgroundColor: window.chartColors.blue,
                borderColor: window.chartColors.blue,
                data: alphaData
            }, {
                label: 'Beta',
                fill: false,
                backgroundColor: window.chartColors.green,
                borderColor: window.chartColors.green,
                borderDash: [5, 5],
                data: betaData,
            }, {
                label: 'Control Group',
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                borderColor: window.chartColors.red,
                data: controlData,
                fill: true,
            }]
        },
        options: {
            responsive: true,
            title: {
                display: true,
                text: 'Line Chart Showing Experiment-Data Report'
            },
            tooltips: {
                mode: 'index',
                intersect: false,
            },
            hover: {
                mode: 'nearest',
                intersect: true
            },
            scales: {
                xAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: 'Month 2020'
                    }
                }],
                yAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: 'Value'
                    }
                }]
            }
        }
    };
    window.threeLineChart = new Chart(ctxThreeLine, lineConfig);

};
function refreshBarOnClick() {
    for (let i = 0; i < 9; i++) {
        graphData[i] = getRandom();
        graphData[i] = Math.abs(graphData[i]);
    }
    window.barChart.update();
}
function refresLinehOnClick() {
    for (let i = 0; i < 6; i++) {
        alphaData[i] = getRandom();
        betaData[i] = getRandom();
        controlData[i] = getRandom();
    }
    window.threeLineChart.update();
}
