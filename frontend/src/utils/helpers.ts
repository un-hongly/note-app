import moment from 'moment'

export function formatDate(
  date: string | Date | null | undefined,
  format = 'MMM D, YYYY hh:mm A'
): string {
  if (!date) return ''
  return moment(date).format(format)
}
