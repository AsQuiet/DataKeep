typedef ColorRGB ColorRGB;
struct ColorRGB
{
int r;
int g;
int b;
int a;
};
void PrintColorRGB (ColorRGB *struct_)
{
printf("%d", struct_->r);
printf("%d", struct_->g);
printf("%d", struct_->b);
printf("alpha : %d", struct_->a);
}
