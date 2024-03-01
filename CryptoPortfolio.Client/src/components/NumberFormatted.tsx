import React from 'react';

interface Props {
  value: number;
  positions: number;
}

const NumberFormatted: React.FC<Props> = ({ value, positions }) => {
  // Format the value to show up to 4 digits after the decimal point without trailing zeroes
  const formattedValue = value.toFixed(positions).replace(/\.?0+$/, '');

  return <>{formattedValue}</>;
};

export default NumberFormatted;