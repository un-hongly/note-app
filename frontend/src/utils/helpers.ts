import moment from 'moment'

export function formatDate(
  date: string | Date | null | undefined,
  format = 'MMM D, YYYY HH:mm'
): string {
  if (!date) return ''
  return moment(date).format(format)
}
