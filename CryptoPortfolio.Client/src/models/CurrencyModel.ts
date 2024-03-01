interface CurrencyModel {
    currencyId: number;
    currencyCode: string;
    initialValue: number;
    currentValue: number;
    numberOfCoins: number;
    changeInPercent: number;
  }

  export default CurrencyModel;