import React from 'react';
import CurrencyModel from '../models/CurrencyModel';
import Title from './Title';
import { Table, TableBody, TableCell, TableHead, TableRow } from '@mui/material';
import NumberFormatted from '../utils/NumberFormatted';

interface CurrencyProps {
  currencyList: CurrencyModel[];
}

const Currency: React.FC<CurrencyProps> = ({ currencyList }) => {
  return (
    <React.Fragment>
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
            {currencyList.map((currency: CurrencyModel) => (
                <TableRow key={currency.currencyId}>
                  <TableCell>{currency.currencyCode}</TableCell>
                  <TableCell>
                    <NumberFormatted value={currency.numberOfCoins || 0} positions={4}/>
                    </TableCell>
                  <TableCell>
                    <NumberFormatted value={currency.currentValue || 0} positions={10}/> USD
                    </TableCell>
                  <TableCell>
                    <NumberFormatted value={currency.changeInPercent || 0} positions={2}/> %
                  </TableCell>
                </TableRow>
            ))}
            </TableBody>
        </Table>
    </React.Fragment>
  );
};

export default Currency;