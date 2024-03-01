import { ChangeEvent, useState, useEffect } from 'react';
import { get, post } from '../services/httpClient';
import { Button, Typography, Container, Grid, Paper} from "@mui/material";
import UploadFileIcon from "@mui/icons-material/UploadFile";
import * as React from 'react';
import PortfolioModel from '../models/PortfolioModel';
import Currency from './Currency';
import Title from './Title';
import NumberFormatted from './NumberFormatted';

const Dashboard: React.FC = () => {
  const [portfolioId, setPortfolioId] = useState(0);
  const [portfolio, setPortfolio] = useState<PortfolioModel | null>();
  const importPortfolio = 'Portfolio/ImportPortfolio';
  const refreshPortfolio = 'Portfolio/RefreshPortfolio?portfolioId=';
  const interval = Number(process.env.REACT_APP_REFRESH_PORTFOLIO_INTERVAL);

  const handleFileUpload = async (e: ChangeEvent<HTMLInputElement>) => {
      if (!e.target.files) {
        return;
      }
      const selectedFile = e.target.files[0];
  
      const formData = new FormData();
      formData.append('file', selectedFile);
  
      try {
        const portfolioIdResponse = await post<number, FormData>(importPortfolio, formData);
        setPortfolioId(portfolioIdResponse);
      } catch (error) {
        setPortfolioId(0);
      }
  }

  const fetchData = async () => {
    if (portfolioId != 0){
      try {
        const response = await get<PortfolioModel>(refreshPortfolio + portfolioId);
        if (response) {
          setPortfolio(response); // Update state after successful response
        }
      } catch (error) {
        console.log("Couldn't get portfolio.");
      }
    }
  };

  useEffect(() => { 
    try {
      fetchData()
      setInterval(fetchData, interval);
    } catch (error) {

    }
  }, [portfolioId]);

  return (
    <React.Fragment>
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
        <Title>Portfolio</Title>
        <Button
          component="label"
          variant="outlined"
          startIcon={<UploadFileIcon />}
          sx={{ marginRight: "1rem", width: '20%' }}
          disabled={portfolio != null}
        >
          Import Portfolio
          <input type="file" accept="text/*" hidden onChange={handleFileUpload}/>
        </Button>
      </div>

      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
            <Grid container spacing={3}>
              <Grid item xs={4}>
                <Paper sx={{ p: 2, display: 'flex', flexDirection: 'column' }}>
                <Typography color="primary" sx={{ flex: 1 }} component="h4" variant="h6">
                 Initial Total
                </Typography>
                <Typography color="text.secondary" sx={{ flex: 1 }}>
                <NumberFormatted value={portfolio?.initialTotal || 0} positions={4}/> USD
                </Typography>
                </Paper>
              </Grid>
              <Grid item xs={4}>
                <Paper sx={{ p: 2, display: 'flex', flexDirection: 'column' }}>
                <Typography color="primary" sx={{ flex: 1 }}>
                  Actual Total
                </Typography>
                <Typography color="text.secondary" sx={{ flex: 1 }}>
                <NumberFormatted value={portfolio?.totalValue || 0} positions={4}/> USD
                </Typography>
                </Paper>
              </Grid>
              <Grid item xs={4}>
                <Paper sx={{ p: 2, display: 'flex', flexDirection: 'column' }}>
                <Typography color="primary" sx={{ flex: 1 }}>
                  Change (%)
                </Typography>
                <Typography color="text.secondary" sx={{ flex: 1, }}>
                  <NumberFormatted value={portfolio?.changeTotalInPercentage || 0} positions={2}/> %
                </Typography>
                </Paper>
              </Grid>
            </Grid>
          </Container>

      { portfolio?.currencies ? <Currency currencyList={portfolio?.currencies || []}/> : null }
    </React.Fragment>
  );
};

export default Dashboard;
