using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
    public class ErrorDto
    {
        public List<String> Errors { get; private set; }
        public bool IsShow { get; private set; }

        public ErrorDto()
        {
            Errors = new List<String>();
        }

        public ErrorDto(string error, bool isShow)
        {
            // We setted all in here.
            Errors.Add(error);
            IsShow = isShow;
        }

        public ErrorDto(List<String> errors, bool isShow)
        {
            // for overload.
            Errors=errors;
            IsShow = isShow;
        }

    }
}
