/// <reference types="react-scripts" />

declare global {
    namespace NodeJS {
      interface ProcessEnv {
        REACT_APP_REFRESH_PORTFOLIO_INTERVAL: number;
        REACT_APP_API_BASE_URL: string;
      }
    }
  }