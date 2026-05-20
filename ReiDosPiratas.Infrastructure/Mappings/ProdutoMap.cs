using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReiDosPiratas.Domain.Entities;

namespace ReiDosPiratas.Infrastructure.Mappings
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("PRODUTOS");

            builder.HasKey(p => p.Produto_ID);
            builder.Property(p => p.Produto_ID).HasColumnName("ID").ValueGeneratedOnAdd();

            builder.Property(p => p.Titulo).HasColumnName("NOME").IsRequired().HasMaxLength(255);
            builder.Property(p => p.Descricao).HasColumnName("DESCRICAO").IsRequired();
            builder.Property(p => p.Imagem_url).HasColumnName("ENDERECO_IMAGEM").IsRequired();

            // Mapeando decimal corretamente para o Oracle
            builder.Property(p => p.Preco).HasColumnName("PRECO").HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(p => p.Preco_original).HasColumnName("PRECO_ORIGINAL").HasColumnType("decimal(18,2)").IsRequired();

            builder.Property(p => p.Peso).HasColumnName("PESO");
            builder.Property(p => p.Estoque).HasColumnName("ESTOQUE").IsRequired();
            builder.Property(p => p.Condicao_produto).HasColumnName("CONDICAO").IsRequired();
            builder.Property(p => p.Altura).HasColumnName("ALTURA").IsRequired();
            builder.Property(p => p.Largura).HasColumnName("LARGURA").IsRequired();
            builder.Property(p => p.Profundidade).HasColumnName("PROFUNDIDADE").IsRequired();
            builder.Property(p => p.FuncionarioId).HasColumnName("FUNCIONARIO_ID").IsRequired();
            builder.Property(p => p.Autor).HasColumnName("AUTOR").IsRequired().HasMaxLength(100);
            builder.Property(p => p.Categoria).HasColumnName("CATEGORIA").IsRequired();
        }
    }
}