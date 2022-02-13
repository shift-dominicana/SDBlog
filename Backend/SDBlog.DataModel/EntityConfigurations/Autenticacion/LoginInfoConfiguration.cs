using SDBlog.DataModel.Autenticacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SDBlog.DataModel.EntityConfigurations.Autenticacion
{
    public class LoginInfoConfiguration : IEntityTypeConfiguration<LoginInfo>
    {
        public void Configure(EntityTypeBuilder<LoginInfo> builder)
        {
            builder.ToTable("LoginInfo", "Autenticacion");
            builder.HasKey(x => x.Id).HasName("LoginInfoId");
            builder.Property(x => x.UsuarioId).IsRequired();
            builder.Property(x => x.TokenAcceso).IsRequired().HasColumnType("Text");
            builder.Property(x => x.ValidoHasta).IsRequired();
            builder.Property(x => x.UsuarioJson).HasColumnType("Text").HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<Usuario>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            );
            builder.Property(x => x.PersonaJson).HasColumnType("Text").HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<Persona>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            );
            builder.Property(x => x.PermisosJson).HasColumnType("Text").HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<IList<Permiso>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            );
        }
    }
}
