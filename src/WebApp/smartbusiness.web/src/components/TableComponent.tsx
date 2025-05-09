import { useEffect, useState } from 'react';
import axios from "axios";
import type { WeatherDto } from '../models/weatherDto';

const TableComponent = () => {
    const [data, setData] = useState<WeatherDto[]>([]);

    useEffect(() => {
        axios.get('http://localhost:2200/WeatherForecast')
            .then(response => setData(response.data))
            .catch(error => console.error('Error fetching data:', error));
    }, [])

    return (
        <div>
            <h1>Weather Forecast</h1>
            <table>
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>TemperatureC</th>
                        <th>TemperatureF</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    {data.map((item, index) => (
                        <tr key={index}>
                            <td>{item.date}</td>
                            <td>{item.temperatureC}</td>
                            <td>{item.summary}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    )
}

export default TableComponent;