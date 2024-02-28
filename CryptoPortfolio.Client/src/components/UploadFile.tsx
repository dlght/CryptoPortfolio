import { ChangeEvent, useState, useEffect } from 'react';
import { get, post } from '../services/httpClient';
import { Button, Box, Typography} from "@mui/material";
import UploadFileIcon from "@mui/icons-material/UploadFile";
import * as React from 'react';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Title from './Title';

interface PortfolioModel {
  portfolioId: number;
  initialTotal: number;
  totalValue: number;
  currencies: CurencyModel[];
  changeTotalInPercentage: string;
}

interface CurencyModel {
  currencyId: number;
  currencyCode: string;
  initialValue: number;
  currentValue: number;
  numberOfCoins: number;
  changeInPercent: string;
}

const FileUpload: React.FC = () => {
  const [portfolioId, setPortfolioId] = useState(0);
  const [portfolio, setPortfolio] = useState<PortfolioModel | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      if (portfolioId != 0){
        try {
          const response = await get<PortfolioModel>('Portfolio/GetPortfolio?portfolioId=' + portfolioId);
          setPortfolio(response); // Update state after successful response

        } catch (error) {
          // Handle errors here (optional)
        }
      }
    };
  
    fetchData(); // Call the function to fetch data and update state
  }, [portfolioId]);

  const handleFileUpload = async (e: ChangeEvent<HTMLInputElement>) => {
      if (!e.target.files) {
        return;
      }
      const selectedFile = e.target.files[0];
  
      const formData = new FormData();
      formData.append('file', selectedFile);
  
      try {
        const response = await post<number, FormData>('Portfolio/upload', formData);
        setPortfolioId(response);
      } catch (error) {
        // Handle errors here (optional)
      }
  }

  return (
    <React.Fragment>
      <Button
        component="label"
        variant="outlined"
        startIcon={<UploadFileIcon />}
        sx={{ marginRight: "1rem", width: '20%' }}
      >
        Upload CSV
        <input type="file" accept="text/*" hidden onChange={handleFileUpload}/>
      </Button>

      <Typography component="p" variant="h4">
        Portfolio
      </Typography>
      <Typography color="text.secondary" sx={{ flex: 1 }}>
        Initial Total: {portfolio?.initialTotal}
      </Typography>
      <Typography color="text.secondary" sx={{ flex: 1 }}>
        Current Total: {portfolio?.totalValue}
      </Typography>
      <Typography color="text.secondary" sx={{ flex: 1 }}>
        Change in %: {portfolio?.changeTotalInPercentage}
      </Typography>

      <Title>Currencies</Title>
      <Table size="small">
        <TableHead>
          <TableRow>
            <TableCell>Currency</TableCell>
            <TableCell>Number Of Coins</TableCell>
            <TableCell>Current Value</TableCell>
            <TableCell>Change In Percent</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {portfolio?.currencies.map((currency: CurencyModel) => (
            <TableRow key={currency.currencyId}>
              <TableCell>{currency.currencyCode}</TableCell>
              <TableCell>{currency.numberOfCoins}</TableCell>
              <TableCell>{currency.currentValue}</TableCell>
              <TableCell>{`${currency.changeInPercent}`}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </React.Fragment>
  );
};

export default FileUpload;
