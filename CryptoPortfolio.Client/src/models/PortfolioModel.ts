import CurrencyModel from "./CurrencyModel";

interface PortfolioModel {
    portfolioId: number;
    initialTotal: number;
    totalValue: number;
    currencies: CurrencyModel[];
    changeTotalInPercentage: number;
  }

  export default PortfolioModel;