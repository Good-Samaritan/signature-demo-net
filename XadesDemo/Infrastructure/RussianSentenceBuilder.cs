using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace XadesDemo.Infrastructure
{
    public class RussianSentenceBuilder : SentenceBuilder
    {
        public override Func<string> RequiredWord
        {
            get { return () => "( Обязательный параметр )"; }
        } 

        public override Func<string> ErrorsHeadingText
        {
            get { return () => "Ошибки: "; }
        }

        public override Func<string> UsageHeadingText
        {
            get { return () => "Используйте: "; }
        }

        public override Func<bool, string> HelpCommandText
        {
            get
            {
                return isOption => isOption
                        ? "Показать детальную информацию о команде"
                        : "Показать справку о програме";
            }
        }

        public override Func<bool, string> VersionCommandText
        {
            get { return _ => "Показать информацию о версиях"; }
        }

        public override Func<Error, string> FormatError
        {
            get
            {
                return error =>
                {
                    switch (error.Tag)
                    {
                        case ErrorType.BadFormatTokenError:
                            return $"Ключ '{((BadFormatTokenError)error).Token}' не распознан";
                        case ErrorType.MissingValueOptionError:
                            return $"Для параметра '{((MissingValueOptionError)error).NameInfo.NameText}' не указано значение.";
                        case ErrorType.UnknownOptionError:
                            return $"Неизвестный параметр: '{((UnknownOptionError)error).Token}'";
                        case ErrorType.MissingRequiredOptionError:
                            var errMisssing = ((MissingRequiredOptionError)error);
                            return errMisssing.NameInfo.Equals(NameInfo.EmptyName)
                                    ? "Обязательное значение не найден"
                                    : $"Обязательный параметр '{errMisssing.NameInfo.NameText}' не найден";
                        case ErrorType.BadFormatConversionError:
                            var badFormat = ((BadFormatConversionError)error);
                            return badFormat.NameInfo.Equals(NameInfo.EmptyName)
                                    ? "Неверное значение параметра"
                                    : $"Параметр '{badFormat.NameInfo.NameText}' имеет неправильный формат";
                        case ErrorType.SequenceOutOfRangeError:
                            var seqOutRange = ((SequenceOutOfRangeError)error);
                            return seqOutRange.NameInfo.Equals(NameInfo.EmptyName)
                                    ? "Неверное количество значений в последовательности"
                                    : $"Последовательность параметров '{seqOutRange.NameInfo.NameText}' имеет неправильное количество значений";
                        case ErrorType.BadVerbSelectedError:
                            return $"Неизвестная команда: '{((BadVerbSelectedError)error).Token}'";
                        case ErrorType.NoVerbSelectedError:
                            return "Команда не выбрана";
                        case ErrorType.RepeatedOptionError:
                            return $"Параметр '{((RepeatedOptionError)error).NameInfo.NameText}' указан более одного раза ";
                    }
                    throw new InvalidOperationException();
                };
            }
        }

        public override Func<IEnumerable<MutuallyExclusiveSetError>, string> FormatMutuallyExclusiveSetErrors
        {
            get
            {
                return errors =>
                {
                    var bySet = from e in errors
                        group e by e.SetName into g
                        select new { SetName = g.Key, Errors = g.ToList() };

                    var msgs = bySet.Select(
                            set =>
                            {
                                var names = string.Join(
                                        string.Empty,
                                        (from e in set.Errors select $"'{e.NameInfo.NameText}', ")).ToArray();
                                var namesCount = set.Errors.Count();

                                var incompat = string.Join(
                                        string.Empty,
                                        (from x in
                                                (from s in bySet where !s.SetName.Equals(set.SetName) from e in s.Errors select e)
                                                .Distinct()
                                            select $"'{x.NameInfo.NameText}', ")).ToArray();

                                return
                                        new StringBuilder("Параметр")
                                                .Append(namesCount > 1 ?  "ы" : "")
                                                .Append(": ")
                                                .Append(names.Take(names.Length - 2))
                                                .Append(' ')
                                                .Append(" не совместимы с ")
                                                .Append(incompat.Take(incompat.Length - 2))
                                                .ToString();
                            }).ToArray();
                    return string.Join(Environment.NewLine, msgs);
                };
            }
        }
    }
}