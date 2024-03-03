export function formatDateWithIntl(date: Date): string {
    const formatter = new Intl.DateTimeFormat('bg', { // Adjust locale if needed
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      hour12: false
    });
  
    return formatter.format(date);
  }